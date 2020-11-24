﻿using TeamCloud.Model.Commands.Core;
using TeamCloud.Model.Data;

namespace TeamCloud.Model.Commands
{
    public sealed class ComponentDeployCommand : DeployCommand<Component, ComponentDeployCommandResult>
    {
        public ComponentDeployCommand(User user, Component payload) : base(user, payload)
        { }
    }
}
