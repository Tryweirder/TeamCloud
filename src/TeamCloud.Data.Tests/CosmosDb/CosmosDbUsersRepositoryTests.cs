﻿/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using TeamCloud.Data.Conditional;
using TeamCloud.Data.CosmosDb.Core;
using TeamCloud.Model.Data;
using Xunit;

namespace TeamCloud.Data.CosmosDb
{
    [Collection(nameof(CosmosDbRepositoryCollection))]
    public class CosmosDbuserRepositoryTests : CosmosDbRepositoryTests<CosmosDbUserRepository>
    {
        private readonly CosmosDbRepositoryFixture fixture;

        public CosmosDbuserRepositoryTests(CosmosDbRepositoryFixture fixture)
            : base(new CosmosDbUserRepository(CosmosDbTestOptions.Instance, Enumerable.Empty<IDocumentExpander>()))
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [ConditionalFact(ConditionalFactPlatforms.Windows)]
        public async Task AddUser()
        {
            var user = await Repository.AddAsync(new User()
            {
                Id = Guid.NewGuid().ToString(),
                Role = OrganizationUserRole.Admin

            }).ConfigureAwait(false);

            AssertContainerDocumentMetadata(user);
        }

        [ConditionalFact(ConditionalFactPlatforms.Windows)]
        public async Task UpdateUser()
        {
            var user = await Repository.AddAsync(new User()
            {
                Id = Guid.NewGuid().ToString(),
                Role = OrganizationUserRole.Admin

            }).ConfigureAwait(false);

            AssertContainerDocumentMetadata(user);

            user.Role = OrganizationUserRole.Member;

            var user2 = await Repository
                .SetAsync(user)
                .ConfigureAwait(false);

            Assert.Equal(user.Id, user2.Id);
            AssertContainerDocumentMetadata(user2);
        }

        [ConditionalFact(ConditionalFactPlatforms.Windows)]
        public async Task RemoveUser()
        {
            var user = await Repository.AddAsync(new User()
            {
                Id = Guid.NewGuid().ToString(),
                Role = OrganizationUserRole.Admin

            }).ConfigureAwait(false);

            AssertContainerDocumentMetadata(user);

            await Repository
                .RemoveAsync(user)
                .ConfigureAwait(false);
        }
    }
}
