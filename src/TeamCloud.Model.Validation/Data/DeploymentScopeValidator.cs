/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System.Linq;
using FluentValidation;
using TeamCloud.Model.Data;

namespace TeamCloud.Model.Validation.Data
{
    public sealed class DeploymentScopeValidator : AbstractValidator<DeploymentScope>
    {
        public DeploymentScopeValidator()
        {
            RuleFor(obj => obj.Organization)
                .MustBeGuid();

            RuleFor(obj => obj.DisplayName)
                .NotEmpty();

            RuleFor(obj => obj.ManagementGroupId)
                .NotEmpty()
                    .When(obj => !(obj.SubscriptionIds?.Any() ?? false))
                    .WithMessage("'{PropertyName}' must be a valid resource ID if no Subscription IDs are provided.");

            RuleFor(obj => obj.SubscriptionIds)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .When(obj => string.IsNullOrEmpty(obj.ManagementGroupId))
                    .WithMessage("'{PropertyName}' must contain at least 1 item/s if no Management Group ID is provided.")
                .ForEach(subscriptionId =>
                {
                    subscriptionId.MustBeGuid();
                });
        }
    }
}
