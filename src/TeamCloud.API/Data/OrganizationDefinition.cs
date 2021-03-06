/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using Newtonsoft.Json;
using TeamCloud.Model.Common;
using TeamCloud.Serialization;

namespace TeamCloud.API.Data
{
    [JsonObject(NamingStrategyType = typeof(TeamCloudNamingStrategy))]
    public class OrganizationDefinition : ISlug, IValidatable
    {
        public string Slug => (this as ISlug).GetSlug();

        [JsonProperty(Required = Required.Always)]
        public string DisplayName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string SubscriptionId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Location { get; set; }

    }
}
