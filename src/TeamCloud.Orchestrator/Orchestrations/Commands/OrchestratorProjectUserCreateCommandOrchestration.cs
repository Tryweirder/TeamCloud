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
using TeamCloud.Model.Data;
using TeamCloud.Orchestration;
using TeamCloud.Orchestrator.Activities;
using TeamCloud.Orchestrator.Entities;
using TeamCloud.Orchestrator.Orchestrations.Utilities;

namespace TeamCloud.Orchestrator.Orchestrations.Commands
{
    public static class OrchestratorProjectUserCreateCommandOrchestration
    {
        [FunctionName(nameof(OrchestratorProjectUserCreateCommandOrchestration))]
        public static async Task RunOrchestration(
            [OrchestrationTrigger] IDurableOrchestrationContext functionContext,
            ILogger log)
        {
            if (functionContext is null)
                throw new ArgumentNullException(nameof(functionContext));

            if (log is null)
                throw new ArgumentNullException(nameof(log));

            var commandMessage = functionContext.GetInput<OrchestratorCommandMessage>();
            var command = (OrchestratorProjectUserCreateCommand)commandMessage.Command;
            var commandResult = command.CreateResult();
            var commandProject = default(Project);

            using (log.BeginCommandScope(command))
            {
                try
                {
                    functionContext.SetCustomStatus($"Creating user", log);

                    using (await functionContext.LockAsync<Project>(command.ProjectId.ToString()).ConfigureAwait(true))
                    {
                        commandProject = await functionContext
                            .GetProjectAsync(command.ProjectId.GetValueOrDefault())
                            .ConfigureAwait(true);

                        commandProject.Users.Add(command.Payload);

                        commandProject = await functionContext
                            .SetProjectAsync(commandProject)
                            .ConfigureAwait(true);
                    }

                    functionContext.SetCustomStatus("Sending commands", log);

                    var providerCommand = new ProviderProjectUserCreateCommand
                    (
                        command.User,
                        command.Payload,
                        commandProject.Id,
                        command.CommandId
                    );

                    var providerResults = await functionContext
                        .SendCommandAsync<ProviderProjectUserCreateCommand>(providerCommand, commandProject)
                        .ConfigureAwait(true);

                    var providerException = providerResults.Values?
                        .GetException();

                    if (providerException != null)
                        throw providerException;
                }
                catch (Exception exc)
                {
                    commandResult.Errors.Add(exc);

                    throw;
                }
                finally
                {
                    var commandException = commandResult.GetException();

                    if (commandException is null)
                        functionContext.SetCustomStatus($"Command succeeded", log);
                    else
                        functionContext.SetCustomStatus($"Command failed", log, commandException);

                    commandResult.Result = command.Payload;

                    functionContext.SetOutput(commandResult);
                }
            }
        }
    }
}
