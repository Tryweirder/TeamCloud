/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCloud.Model.Data;

namespace TeamCloud.Data
{
    public interface IComponentRepository : IDocumentRepository<Component>
    {
        Task RemoveAllAsync(string projectId, bool soft);

        Task<Component> RemoveAsync(Component component, bool soft);

        IAsyncEnumerable<Component> ListAsync(string projectId, bool includeDeleted);
    }
}
