﻿/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TeamCloud.Model.Common;
using TeamCloud.Model.Data.Serialization;

namespace TeamCloud.Model.Data.Core
{
    [JsonConverter(typeof(ContainerDocumentConverter))]
    public interface IContainerDocument : IIdentifiable, IValidatable
    {
        [DatabaseIgnore]
        [JsonProperty("_timestamp")]
        DateTime? Timestamp { get; set; }

        [DatabaseIgnore]
        [JsonProperty("_etag")]
        string ETag { get; set; }
    }

    public abstract class ContainerDocument : IContainerDocument
    {
        [JsonProperty(Required = Required.Always)]
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();

        DateTime? IContainerDocument.Timestamp { get; set; }

        string IContainerDocument.ETag { get; set; }

        public override string ToString()
        {
            if (this is IProjectContext projectContext)
                return $"/orgs/{projectContext.Organization}/projects/{projectContext.ProjectId}/{this.GetType().Name.ToLowerInvariant()}s/{this.Id}";
            else if (this is IOrganizationContext organizationContext)
                return $"/orgs/{organizationContext.Organization}/{this.GetType().Name.ToLowerInvariant()}s/{this.Id}";
            else
                return base.ToString();
        }
    }
}
