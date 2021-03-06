/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TeamCloud.Model.Common;
using TeamCloud.Model.Data.Core;
using TeamCloud.Serialization;

namespace TeamCloud.Model.Data
{
    [JsonObject(NamingStrategyType = typeof(TeamCloudNamingStrategy))]
    public sealed class DeploymentScope : ContainerDocument, IOrganizationContext, ISlug, IEquatable<DeploymentScope>, IValidatable
    {
        [PartitionKey]
        [JsonProperty(Required = Required.Always)]
        public string Organization { get; set; }

        [UniqueKey]
        [JsonProperty(Required = Required.Always)]
        public string Slug => (this as ISlug).GetSlug();

        [JsonProperty(Required = Required.Always)]
        public string DisplayName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool IsDefault { get; set; }

        public string ManagementGroupId { get; set; }

        public List<Guid> SubscriptionIds { get; set; }


        public bool Equals(DeploymentScope other)
            => Id.Equals(other?.Id, StringComparison.Ordinal);

        public override bool Equals(object obj)
            => base.Equals(obj) || Equals(obj as DeploymentScope);

        public override int GetHashCode()
            => Id?.GetHashCode(StringComparison.Ordinal) ?? base.GetHashCode();
    }
}
