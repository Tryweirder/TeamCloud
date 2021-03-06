/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using TeamCloud.Git.Data;
using TeamCloud.Model.Data;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TeamCloud.Git.Services
{
    internal class DevOpsService
    {
        private readonly IDeserializer yamlDeserializer;

        internal DevOpsService()
        {
            yamlDeserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        }

        internal async Task<ProjectTemplate> UpdateProjectTemplateAsync(ProjectTemplate projectTemplate)
        {
            if (projectTemplate is null)
                throw new ArgumentNullException(nameof(projectTemplate));

            var repository = projectTemplate.Repository;

            var creds = new VssBasicCredential(string.Empty, repository.Token);
            using var connection = new VssConnection(new Uri(repository.BaselUrl), creds);
            using var client = connection.GetClient<GitHttpClient>();

            var commit = await client
                .GetCommitAsync(project: repository.Project, repository.Ref, repository.Repository)
                .ConfigureAwait(false);

            var result = await client
                .GetTreeAsync(project: repository.Project, repository.Repository, commit.TreeId, recursive: true)
                .ConfigureAwait(false);

            var projectYamlFileStream = await client
                .GetItemContentAsync(project: repository.Project, repository.Repository, Constants.ProjectYaml, download: true, versionDescriptor: repository.VersionDescriptor())
                .ConfigureAwait(false);

            using var streamReader = new StreamReader(projectYamlFileStream);

            var projectYamlFile = await streamReader
                .ReadToEndAsync()
                .ConfigureAwait(false);

            var projectYaml = yamlDeserializer.Deserialize<ProjectYaml>(projectYamlFile);

            projectYaml.Description = await CheckAndPopulateFileContentAsync(client, repository, result.TreeEntries, projectYaml.Description)
                .ConfigureAwait(false);

            return projectTemplate.UpdateFromYaml(projectYaml);
        }


        internal async IAsyncEnumerable<ComponentTemplate> GetComponentTemplatesAsync(ProjectTemplate projectTemplate)
        {
            if (projectTemplate is null)
                throw new ArgumentNullException(nameof(projectTemplate));

            var repository = projectTemplate.Repository;

            var creds = new VssBasicCredential(string.Empty, repository.Token);
            using var connection = new VssConnection(new Uri(repository.BaselUrl), creds);
            using var client = connection.GetClient<GitHttpClient>();

            var commit = await client
                .GetCommitAsync(project: repository.Project, repository.Ref, repository.Repository)
                .ConfigureAwait(false);

            var result = await client
                .GetTreeAsync(project: repository.Project, repository.Repository, commit.TreeId, recursive: true)
                .ConfigureAwait(false);

            var componentTemplates = result.TreeEntries
                .Where(t => t.RelativePath.EndsWith(Constants.ComponentYaml, StringComparison.Ordinal))
                .Select(te => ResolveComponentTemplate(te))
                .ToAsyncEnumerable();

            await foreach (var componentTemplate in componentTemplates)
                yield return componentTemplate;

            async Task<ComponentTemplate> ResolveComponentTemplate(GitTreeEntryRef componentItem)
            {
                var folder = componentItem.RelativePath.Split(Constants.ComponentYaml).First().TrimEnd('/');

                var componentFileStream = await client
                    .GetItemContentAsync(project: repository.Project, repository.Repository, componentItem.RelativePath, download: true, versionDescriptor: repository.VersionDescriptor())
                    .ConfigureAwait(false);

                using var streamReader = new StreamReader(componentFileStream);

                var componentFile = await streamReader
                    .ReadToEndAsync()
                    .ConfigureAwait(false);

                var componentYaml = yamlDeserializer.Deserialize<ComponentYaml>(componentFile);

                componentYaml.Description = await CheckAndPopulateFileContentAsync(client, repository, result.TreeEntries, componentYaml.Description, folder)
                    .ConfigureAwait(false);

                return componentYaml.ToTemplate(projectTemplate, folder);
            }
        }

        private static async Task<string> CheckAndPopulateFileContentAsync(GitHttpClient client, RepositoryReference repo, IEnumerable<GitTreeEntryRef> tree, string value, string folder = null)
        {
            if (string.IsNullOrEmpty(value) || !Uri.IsWellFormedUriString(value, UriKind.Relative))
                return value;

            var fileName = value.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
            var filePath = string.IsNullOrEmpty(folder) ? fileName : $"{folder}/{fileName}";
            var fileItem = tree.FirstOrDefault(i => i.RelativePath.Equals(filePath, StringComparison.Ordinal));

            if (fileItem is null)
                return value;

            var fileStream = await client
                .GetItemContentAsync(project: repo.Project, repo.Repository, fileItem.RelativePath, download: true, versionDescriptor: repo.VersionDescriptor())
                .ConfigureAwait(false);

            using var streamReader = new StreamReader(fileStream);

            var file = await streamReader
                .ReadToEndAsync()
                .ConfigureAwait(false);

            return string.IsNullOrEmpty(file) ? value : file;
        }

        internal static async Task<RepositoryReference> GetRepositoryReferenceAsync(RepositoryReference repository)
        {
            if (repository is null)
                throw new ArgumentNullException(nameof(repository));

            if (!string.IsNullOrEmpty(repository.Version) && Constants.ValidSha1.IsMatch(repository.Version))
            {
                repository.Ref = repository.Version;
                repository.Type = RepositoryReferenceType.Hash;

                return repository;
            }

            var creds = new VssBasicCredential(string.Empty, repository.Token);

            using var connection = new VssConnection(new Uri(repository.BaselUrl), creds);
            using var client = connection.GetClient<GitHttpClient>();

            var refs = await client
                .GetRefsAsync(project: repository.Project, repository.Repository, filterContains: repository.Version ?? "", peelTags: true)
                .ConfigureAwait(false);

            if (!(refs?.Any() ?? false))
                throw new Exception("No matching ref found");

            // use latest tag
            if (string.IsNullOrEmpty(repository.Version))
            {
                var tags = refs.Where(r => r.IsTag()).ToList();

                if (!(tags?.Any() ?? false)) // TODO: maybe just use master/main
                    throw new Exception("No tags found");

                tags.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));

                var tag = tags.Last();

                repository.Ref = tag.PeeledObjectId;
                repository.Version = tag.Name;
                repository.Type = RepositoryReferenceType.Tag;
            }
            else
            {
                var gitRef = refs.First();

                repository.Ref = string.IsNullOrEmpty(gitRef.PeeledObjectId) ? gitRef.ObjectId : gitRef.PeeledObjectId;

                if (gitRef.IsTag())
                    repository.Type = RepositoryReferenceType.Tag;

                if (gitRef.IsBranch())
                    repository.Type = RepositoryReferenceType.Branch;
            }

            return repository;
        }
    }

    internal static class DevOpsServiceExtensions
    {
        public static GitVersionDescriptor VersionDescriptor(this RepositoryReference repository)
            => new GitVersionDescriptor
            {
                Version = repository.Ref,
                VersionType = GitVersionType.Commit
            };
    }
}
