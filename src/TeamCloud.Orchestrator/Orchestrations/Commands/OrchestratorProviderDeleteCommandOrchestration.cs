/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using TeamCloud.Model;
using TeamCloud.Model.Commands;
using TeamCloud.Model.Commands.Core;
using TeamCloud.Orchestration;
using TeamCloud.Orchestrator.Activities;
using TeamCloud.Orchestrator.Entities;
using TeamCloud.Orchestrator.Options;

namespace TeamCloud.Orchestrator.Orchestrations.Commands
{
    public class OrchestratorProviderDeleteCommandOrchestration
    {
        private readonly OrchestratorDatabaseOptions orchestratorDatabaseOptions;

        public OrchestratorProviderDeleteCommandOrchestration(OrchestratorDatabaseOptions orchestratorDatabaseOptions)
        {
            this.orchestratorDatabaseOptions = orchestratorDatabaseOptions ?? throw new ArgumentNullException(nameof(orchestratorDatabaseOptions));
        }

        [FunctionName(nameof(OrchestratorProviderDeleteCommandOrchestration))]
        public static async Task RunOrchestration(
            [OrchestrationTrigger] IDurableOrchestrationContext functionContext,
            ILogger log)
        {
            if (functionContext is null)
                throw new ArgumentNullException(nameof(functionContext));

            if (log is null)
                throw new ArgumentNullException(nameof(log));

            var command = functionContext.GetInput<OrchestratorProviderDeleteCommand>();
            var commandResult = command.CreateResult();
            var provider = command.Payload;

            using (log.BeginCommandScope(command, provider))
            {
                try
                {
                    using (await functionContext.LockContainerDocumentAsync(provider).ConfigureAwait(true))
                    {
                        if (!(provider is null))
                            await functionContext
                                .DeleteProviderAsync(provider)
                                .ConfigureAwait(true);
                    }
                }
                catch (Exception exc)
                {
                    commandResult ??= command.CreateResult();
                    commandResult.Errors.Add(exc);

                    throw;
                }
                finally
                {
                    var commandException = commandResult.Errors?.ToException();

                    if (commandException is null)
                        functionContext.SetCustomStatus($"Command succeeded", log);
                    else
                        functionContext.SetCustomStatus($"Command failed: {commandException.Message}", log, commandException);

                    functionContext.SetOutput(commandResult);
                }
            }
        }
    }
}
