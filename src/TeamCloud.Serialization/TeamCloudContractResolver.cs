﻿/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Reflection;
using TeamCloud.Serialization.Converter;
using TeamCloud.Serialization.Encryption;

namespace TeamCloud.Serialization
{
    public class TeamCloudContractResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly IDataProtectionProvider dataProtectionProvider;

        public TeamCloudContractResolver(IDataProtectionProvider dataProtectionProvider = null)
        {
            // prevent changing the case of dictionary keys
            NamingStrategy = new TeamCloudNamingStrategy();

            this.dataProtectionProvider = dataProtectionProvider;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            return contract;
        }

        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (objectType is null)
                throw new ArgumentNullException(nameof(objectType));

            if (typeof(Exception).IsAssignableFrom(objectType))
                return new ExceptionConverter();

            if (objectType.IsEnum)
                return new StringEnumConverter();

            return base.ResolveContractConverter(objectType);
        }

        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            var valueProvider = base.CreateMemberValueProvider(member);

            return member.GetCustomAttribute<EncryptedAttribute>() is null
                ? valueProvider // not marked as encrypted - no need to wrap the value provider
                : new EncryptedValueProvider(member, valueProvider, dataProtectionProvider);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (member is PropertyInfo propertyInfo)
            {
                if (!prop.Writable)
                {
                    // enable private property setter deserialization for types with default constructor
                    prop.Writable = propertyInfo.GetSetMethod(true) != null;
                }

                // suppress serialization of empty enumerations
                if (propertyInfo.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    bool shouldSerializeEnumerable(object obj)
                    {
                        var enumerable = prop.ValueProvider.GetValue(obj) as IEnumerable;
                        return enumerable?.GetEnumerator().MoveNext() ?? false;
                    }

                    prop.ShouldSerialize = prop.ShouldSerialize == null
                        ? (Predicate<object>)shouldSerializeEnumerable
                        : obj => prop.ShouldSerialize(obj) && shouldSerializeEnumerable(obj);
                }
            }

            return prop;
        }
    }
}
