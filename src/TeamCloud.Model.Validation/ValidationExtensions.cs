﻿/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using TeamCloud.Model.Data;

namespace TeamCloud.Model.Validation
{
    public static class ValidationExtensions
    {
        private static IEnumerable<Type> GetValidatorTargetTypes(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
                return Enumerable.Empty<Type>();

            return validatorType.GetInterfaces()
                .Where(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IValidator<>))
                .Select(type => type.GetGenericArguments().First());
        }

        private static readonly Dictionary<Type, Type[]> Validators = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.IsClass && typeof(IValidator).IsAssignableFrom(type))
            .SelectMany(validatorType => GetValidatorTargetTypes(validatorType).Select(targetType => new KeyValuePair<Type, Type>(targetType, validatorType)))
            .GroupBy(kvp => kvp.Key)
            .ToDictionary(grp => grp.Key, grp => grp.Select(item => item.Value).ToArray());

        private static IEnumerable<Type> GetValidators(Type validationTargetType)
        {
            var validatorTypes = new List<Type>();

            if (Validators.TryGetValue(validationTargetType, out var validatorTypesByClass))
                validatorTypes.AddRange(validatorTypesByClass);

            foreach (var validationTargetInterfaceType in validationTargetType.GetInterfaces())
            {
                if (Validators.TryGetValue(validationTargetInterfaceType, out var validatorTypesByInterface))
                    validatorTypes.AddRange(validatorTypesByInterface);
            }

            if (validationTargetType.BaseType != null)
                validatorTypes.AddRange(GetValidators(validationTargetType.BaseType));

            return validatorTypes.Distinct();
        }

        public static ValidationResult Validate(this IValidatable validatable, IServiceProvider provider = null, bool throwOnNoValidatorFound = false)
        {
            if (validatable is null)
                throw new ArgumentNullException(nameof(validatable));

            var validatorTypes = GetValidators(validatable.GetType());

            if (validatorTypes.Any())
            {
                return validatorTypes
                    .Select(validatorType => ValidateInternal(validatorType, validatable, provider))
                    .MergeValidationResults();
            }

            if (throwOnNoValidatorFound)
                throw new NotSupportedException($"Validation of type {validatable.GetType()} is not supported");

            return new ValidationResult();
        }

        public static async Task<ValidationResult> ValidateAsync(this IValidatable validatable, IServiceProvider provider = null, bool throwOnNoValidatorFound = false)
        {
            if (validatable is null)
                throw new ArgumentNullException(nameof(validatable));

            var validatorTypes = GetValidators(validatable.GetType());

            if (validatorTypes.Any())
            {
                var validationTasks = validatorTypes
                    .Select(validatorType => ValidateInternalAsync(validatorType, validatable, provider));

                var validationResults = await Task
                    .WhenAll(validationTasks)
                    .ConfigureAwait(false);

                return validationResults
                    .MergeValidationResults();
            }

            if (throwOnNoValidatorFound)
                throw new NotSupportedException($"Validation of type {validatable.GetType()} is not supported");

            return new ValidationResult();
        }

        private static ValidationResult MergeValidationResults(this IEnumerable<ValidationResult> validationResults)
        {
            var failures = validationResults
                .SelectMany(validationResult => validationResult.Errors);

            return new ValidationResult(failures);
        }

        private static ValidationResult ValidateInternal(Type validatorType, object instance, IServiceProvider provider)
        {
            var validatorInstance = (IValidator)(provider is null
                ? Activator.CreateInstance(validatorType)
                : ActivatorUtilities.CreateInstance(provider, validatorType));

            return validatorInstance.Validate(instance);
        }

        private static Task<ValidationResult> ValidateInternalAsync(Type validatorType, object instance, IServiceProvider provider)
        {
            var validatorInstance = (IValidator)(provider is null
                ? Activator.CreateInstance(validatorType)
                : ActivatorUtilities.CreateInstance(provider, validatorType));

            return validatorInstance.ValidateAsync(instance);
        }

        public static IRuleBuilderOptions<T, IList<TElement>> MustContainAtLeast<T, TElement>(this IRuleBuilderInitial<T, IList<TElement>> ruleBuilder, int min)
            => ruleBuilder
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(list => list.Count >= min)
                    .WithMessage("'{PropertyName}' must contain at least " + $"{min} items.");

        public static IRuleBuilderOptions<T, string> MustBeResourcId<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
            => ruleBuilder
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(BeValidResourceId)
                    .WithMessage("'{PropertyName}' must be less than 255 characters long and may not contain: " + @"'/', '\\', '?', '#'");

        public static IRuleBuilderOptions<T, string> MustBeAzureRegion<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
            => ruleBuilder
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(BeAzureRegion)
                    .WithMessage("'{PropertyName}' must be a valid Azure Region. See https://azure.microsoft.com/en-us/global-infrastructure/regions/ for more information on Azure Regions");


        public static IRuleBuilderOptions<T, string> MustBeEmail<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
            => ruleBuilder
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .EmailAddress()
                    .WithMessage("'{PropertyName}' must be a valid email address.");


        public static IRuleBuilderOptions<T, string> MustBeGuid<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
            => ruleBuilder
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(BeGuid)
                    .WithMessage("'{PropertyName}' must be a valid, non-empty GUID.");

        public static IRuleBuilderOptions<T, Guid> MustBeGuid<T>(this IRuleBuilder<T, Guid> ruleBuilder)
            => ruleBuilder
                .NotEqual(Guid.Empty)
                    .WithMessage("'{PropertyName}' must be a valid, non-empty GUID.");


        public static IRuleBuilderOptions<T, string> MustBeUrl<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
            => ruleBuilder
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(BeUrl)
                    .WithMessage("'{PropertyName}' must be a valid url.");

        public static IRuleBuilderOptions<T, string> MustBeUserRole<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
            => ruleBuilder
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(BeUserRole)
                    .WithMessage("'{PropertyName}' must be a valid Role. Valid roles for Project users are 'Owner' and 'Member'. Valid roles for TeamCloud users are 'Admin' and 'Creator'.");


        private static bool BeGuid(string guid)
            => !string.IsNullOrEmpty(guid) && Guid.TryParse(guid, out var outGuid) && !outGuid.Equals(Guid.Empty);

        private static bool BeUrl(string url)
            => !string.IsNullOrEmpty(url) && Uri.TryCreate(url, UriKind.Absolute, out var _);

        private static bool BeUserRole(string role)
            => !string.IsNullOrEmpty(role) && ValidUserRoles.Contains(role.ToLowerInvariant());

        private static bool BeAzureRegion(string region)
            => !string.IsNullOrEmpty(region) && AzureRegion.IsValid(region);

        private static bool BeValidResourceId(string id)
            => !(string.IsNullOrEmpty(id) || id.Length >= 255 || id.Contains('/') || id.Contains(@"\\") || id.Contains('?') || id.Contains('#'));


        private static readonly string[] ValidUserRoles = new string[]
        {
            UserRoles.Project.Owner.ToLowerInvariant(), UserRoles.Project.Member.ToLowerInvariant(), UserRoles.TeamCloud.Admin.ToLowerInvariant(), UserRoles.TeamCloud.Creator.ToLowerInvariant()
        };
    }

    internal class AzureRegion
    {
        private static ConcurrentDictionary<string, AzureRegion> regions = new ConcurrentDictionary<string, AzureRegion>();

        #region Americas
        internal static readonly AzureRegion USWest = new AzureRegion("westus");
        internal static readonly AzureRegion USWest2 = new AzureRegion("westus2");
        internal static readonly AzureRegion USCentral = new AzureRegion("centralus");
        internal static readonly AzureRegion USEast = new AzureRegion("eastus");
        internal static readonly AzureRegion USEast2 = new AzureRegion("eastus2");
        internal static readonly AzureRegion USNorthCentral = new AzureRegion("northcentralus");
        internal static readonly AzureRegion USSouthCentral = new AzureRegion("southcentralus");
        internal static readonly AzureRegion USWestCentral = new AzureRegion("westcentralus");
        internal static readonly AzureRegion CanadaCentral = new AzureRegion("canadacentral");
        internal static readonly AzureRegion CanadaEast = new AzureRegion("canadaeast");
        internal static readonly AzureRegion BrazilSouth = new AzureRegion("brazilsouth");
        #endregion

        #region Europe
        internal static readonly AzureRegion EuropeNorth = new AzureRegion("northeurope");
        internal static readonly AzureRegion EuropeWest = new AzureRegion("westeurope");
        internal static readonly AzureRegion UKSouth = new AzureRegion("uksouth");
        internal static readonly AzureRegion UKWest = new AzureRegion("ukwest");
        internal static readonly AzureRegion FranceCentral = new AzureRegion("francecentral");
        internal static readonly AzureRegion FranceSouth = new AzureRegion("francesouth");
        internal static readonly AzureRegion SwitzerlandNorth = new AzureRegion("switzerlandnorth");
        internal static readonly AzureRegion SwitzerlandWest = new AzureRegion("switzerlandwest");
        internal static readonly AzureRegion GermanyNorth = new AzureRegion("germanynorth");
        internal static readonly AzureRegion GermanyWestCentral = new AzureRegion("germanywestcentral");
        internal static readonly AzureRegion NorwayWest = new AzureRegion("norwaywest");
        internal static readonly AzureRegion NorwayEast = new AzureRegion("norwayeast");
        #endregion

        #region Asia
        internal static readonly AzureRegion AsiaEast = new AzureRegion("eastasia");
        internal static readonly AzureRegion AsiaSouthEast = new AzureRegion("southeastasia");
        internal static readonly AzureRegion JapanEast = new AzureRegion("japaneast");
        internal static readonly AzureRegion JapanWest = new AzureRegion("japanwest");
        internal static readonly AzureRegion AustraliaEast = new AzureRegion("australiaeast");
        internal static readonly AzureRegion AustraliaSouthEast = new AzureRegion("australiasoutheast");
        internal static readonly AzureRegion AustraliaCentral = new AzureRegion("australiacentral");
        internal static readonly AzureRegion AustraliaCentral2 = new AzureRegion("australiacentral2");
        internal static readonly AzureRegion IndiaCentral = new AzureRegion("centralindia");
        internal static readonly AzureRegion IndiaSouth = new AzureRegion("southindia");
        internal static readonly AzureRegion IndiaWest = new AzureRegion("westindia");
        internal static readonly AzureRegion KoreaSouth = new AzureRegion("koreasouth");
        internal static readonly AzureRegion KoreaCentral = new AzureRegion("koreacentral");
        #endregion

        #region Middle East and Africa
        internal static readonly AzureRegion UAECentral = new AzureRegion("uaecentral");
        internal static readonly AzureRegion UAENorth = new AzureRegion("uaenorth");
        internal static readonly AzureRegion SouthAfricaNorth = new AzureRegion("southafricanorth");
        internal static readonly AzureRegion SouthAfricaWest = new AzureRegion("southafricawest");
        #endregion

        #region China
        internal static readonly AzureRegion ChinaNorth = new AzureRegion("chinanorth");
        internal static readonly AzureRegion ChinaEast = new AzureRegion("chinaeast");
        internal static readonly AzureRegion ChinaNorth2 = new AzureRegion("chinanorth2");
        internal static readonly AzureRegion ChinaEast2 = new AzureRegion("chinaeast2");
        #endregion

        #region German
        internal static readonly AzureRegion GermanyCentral = new AzureRegion("germanycentral");
        internal static readonly AzureRegion GermanyNorthEast = new AzureRegion("germanynortheast");
        #endregion

        #region Government Cloud
        /// <summary>
        /// U.S. government cloud in Virginia.
        /// </summary>
        internal static readonly AzureRegion GovernmentUSVirginia = new AzureRegion("usgovvirginia");

        /// <summary>
        /// U.S. government cloud in Iowa.
        /// </summary>
        internal static readonly AzureRegion GovernmentUSIowa = new AzureRegion("usgoviowa");

        /// <summary>
        /// U.S. government cloud in Arizona.
        /// </summary>
        internal static readonly AzureRegion GovernmentUSArizona = new AzureRegion("usgovarizona");

        /// <summary>
        /// U.S. government cloud in Texas.
        /// </summary>
        internal static readonly AzureRegion GovernmentUSTexas = new AzureRegion("usgovtexas");

        /// <summary>
        /// U.S. Department of Defense cloud - East.
        /// </summary>
        internal static readonly AzureRegion GovernmentUSDodEast = new AzureRegion("usdodeast");

        /// <summary>
        /// U.S. Department of Defense cloud - Central.
        /// </summary>
        internal static readonly AzureRegion GovernmentUSDodCentral = new AzureRegion("usdodcentral");

        #endregion

        internal static IReadOnlyCollection<AzureRegion> Values
            => regions.Values as IReadOnlyCollection<AzureRegion>;

        internal string Name
        {
            get; private set;
        }

        private AzureRegion(string name)
        {
            Name = name.ToLowerInvariant();
            regions.AddOrUpdate(Name, this, (k, v) => v);
        }

        internal static bool IsValid(string name)
            => regions.ContainsKey(name.Replace(" ", "").ToLowerInvariant());

        public override int GetHashCode() => this.Name.GetHashCode();

        public static bool operator ==(AzureRegion lhs, AzureRegion rhs)
            => (object.ReferenceEquals(lhs, null))
                ? object.ReferenceEquals(rhs, null)
                : lhs.Equals(rhs);

        public static bool operator !=(AzureRegion lhs, AzureRegion rhs) => !(lhs == rhs);

        public override bool Equals(object obj)
        {
            if (!(obj is AzureRegion))
                return false;

            if (object.ReferenceEquals(obj, this))
                return true;

            AzureRegion rhs = (AzureRegion)obj;

            if (Name is null)
                return rhs.Name is null;

            return Name.Equals(rhs.Name, System.StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString() => this.Name;
    }
}
