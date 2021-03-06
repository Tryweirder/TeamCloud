﻿/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using TeamCloud.Azure.Resources;
using TeamCloud.Data;
using TeamCloud.Model.Common;
using TeamCloud.Model.Data;
using TeamCloud.Orchestration;
using TeamCloud.Orchestrator.Operations.Activities;
using TeamCloud.Orchestrator.Operations.Entities;
using TeamCloud.Serialization;

namespace TeamCloud.Orchestrator.Operations.Orchestrations.Utilities
{
    public sealed class ComponentPrepareOrchestration
    {
        private readonly IComponentRepository componentRepository;
        private readonly IUserRepository userRepository;
        private readonly IAzureResourceService azureResourceService;

        public ComponentPrepareOrchestration(IComponentRepository componentRepository, IUserRepository userRepository, IAzureResourceService azureResourceService)
        {
            this.componentRepository = componentRepository ?? throw new ArgumentNullException(nameof(componentRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.azureResourceService = azureResourceService ?? throw new ArgumentNullException(nameof(azureResourceService));
        }

        [FunctionName(nameof(ComponentPrepareOrchestration))]
        public async Task<Component> Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (log is null)
                throw new ArgumentNullException(nameof(log));

            var component = context.GetInput<Input>().Component;

            try
            {
                var ready = await context
                    .CallActivityWithRetryAsync<bool>(nameof(ComponentGuardActivity), new ComponentGuardActivity.Input() { Component = component })
                    .ConfigureAwait(true);

                if (!ready)
                {
                    context
                        .CreateReplaySafeLogger(log)
                        .LogInformation($"!!! Component deployment '{component}' needs to wait for Organization/Project resource to be provisioned.");

                    await context
                        .ContinueAsNew(new Input() { Component = component }, TimeSpan.FromSeconds(2))
                        .ConfigureAwait(true);
                }

                using (await context.LockContainerDocumentAsync(component, nameof(ComponentPrepareOrchestration)).ConfigureAwait(true))
                {
                    component = (await context
                        .CallActivityWithRetryAsync<Component>(nameof(ComponentGetActivity), new ComponentGetActivity.Input() { ProjectId = component.ProjectId, ComponentId = component.Id })
                        .ConfigureAwait(true)) ?? component;

                    if (!component.ResourceState.IsFinal())
                    {
                        component = await UpdateComponentAsync(component, ResourceState.Provisioning)
                            .ConfigureAwait(true);
                    }

                    component = await context
                        .CallActivityWithRetryAsync<Component>(nameof(ComponentEnsureIdentityActivity), new ComponentEnsureIdentityActivity.Input() { Component = component })
                        .ConfigureAwait(true);

                    component = await context
                        .CallActivityWithRetryAsync<Component>(nameof(ComponentEnsureResourceActivity), new ComponentEnsureResourceActivity.Input() { Component = component })
                        .ConfigureAwait(true);

                    component = await context
                        .CallActivityWithRetryAsync<Component>(nameof(ComponentEnsureStorageActivity), new ComponentEnsureStorageActivity.Input() { Component = component })
                        .ConfigureAwait(true);

                    component = await context
                        .CallActivityWithRetryAsync<Component>(nameof(ComponentEnsureVaultActivity), new ComponentEnsureVaultActivity.Input() { Component = component })
                        .ConfigureAwait(true);

                    component = await context
                        .CallActivityWithRetryAsync<Component>(nameof(ComponentEnsurePermissionActivity), new ComponentEnsurePermissionActivity.Input() { Component = component })
                        .ConfigureAwait(true);

                    component = await UpdateComponentAsync(component, ResourceState.Succeeded)
                        .ConfigureAwait(true);
                }

                return component;
            }
            catch (Exception exc)
            {
                component = await UpdateComponentAsync(component, ResourceState.Failed)
                    .ConfigureAwait(true);

                throw exc.AsSerializable();
            }

            Task<Component> UpdateComponentAsync(Component component, ResourceState? resourceState = null)
            {
                component.ResourceState = resourceState.GetValueOrDefault(component.ResourceState);

                return context.CallActivityWithRetryAsync<Component>(nameof(ComponentSetActivity), new ComponentSetActivity.Input() { Component = component });
            }
        }

        internal struct Input
        {
            public Component Component { get; set; }
        }
    }
}
