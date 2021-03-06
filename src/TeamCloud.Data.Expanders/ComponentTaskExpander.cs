/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeamCloud.Azure.Resources;
using TeamCloud.Azure.Resources.Typed;
using TeamCloud.Model.Data;

namespace TeamCloud.Data.Expanders
{
    public sealed class ComponentTaskExpander : IDocumentExpander<ComponentTask>
    {
        private readonly IAzureResourceService azureResourceService;

        public ComponentTaskExpander(IAzureResourceService azureResourceService)
        {
            this.azureResourceService = azureResourceService ?? throw new ArgumentNullException(nameof(azureResourceService));
        }

        public bool CanExpand(ComponentTask document)
        {
            if (document is null)
                throw new ArgumentNullException(nameof(document));

            return string.IsNullOrEmpty(document.Output);
        }

        public async Task<ComponentTask> ExpandAsync(ComponentTask document)
        {
            if (document is null)
                throw new ArgumentNullException(nameof(document));

            var results = await Task.WhenAll(

                GetEventsAsync(document),
                GetOutputAsync(document)

            ).ConfigureAwait(false);

            if (results.Any(result => !string.IsNullOrEmpty(result)))
            {
                document.Output = string.Join(Environment.NewLine, results); // do some empty line trimming (left & right)
                document.Output = Regex.Replace(document.Output, @"^([\s])*", string.Empty, RegexOptions.Singleline);
                document.Output = Regex.Replace(document.Output, @"([\s])*$", string.Empty, RegexOptions.Singleline);
            }

            return document;
        }

        private async Task<string> GetEventsAsync(ComponentTask document)
        {
            if (AzureResourceIdentifier.TryParse(document.ResourceId, out var resourceId))
            {
                try
                {
                    var session = await azureResourceService.AzureSessionService
                        .CreateSessionAsync(resourceId.SubscriptionId)
                        .ConfigureAwait(false);

                    var runner = await session.ContainerGroups
                        .GetByIdAsync(resourceId.ToString())
                        .ConfigureAwait(false);

                    var container = runner?.Containers
                        .SingleOrDefault()
                        .Value;

                    if (container?.InstanceView != null)
                    {
                        var lines = container.InstanceView.Events
                            .Where(e => e.LastTimestamp.HasValue)
                            .OrderBy(e => e.LastTimestamp)
                            .Select(e => $"{e.LastTimestamp.Value:yyyy-MM-dd hh:mm:ss}\t{e.Name}\t\t{e.Message}");

                        if (lines.Any())
                            lines = lines.Append(string.Empty);

                        return string.Join(Environment.NewLine, lines);
                    }
                }
                catch
                {
                    // swallow
                }
            }

            return default;
        }

        private async Task<string> GetOutputAsync(ComponentTask document)
        {
            if (AzureResourceIdentifier.TryParse(document.StorageId, out var storageId))
            {
                try
                {
                    var storageAccount = await azureResourceService
                    .GetResourceAsync<AzureStorageAccountResource>(storageId.ToString(), false)
                    .ConfigureAwait(false);

                    if (storageAccount != null)
                    {
                        var shareClient = await storageAccount
                            .CreateShareClientAsync(document.ComponentId)
                            .ConfigureAwait(false);

                        var fileClient = shareClient
                            .GetRootDirectoryClient()
                            .GetFileClient($"{document.Id}.log");

                        if (await fileClient.ExistsAsync().ConfigureAwait(false))
                        {
                            using var reader = new StreamReader(await fileClient.OpenReadAsync().ConfigureAwait(false));

                            return await reader.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }
                }
                catch
                {
                    // swallow
                }
            }

            return default;
        }
    }
}
