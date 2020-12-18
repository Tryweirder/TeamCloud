﻿/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using TeamCloud.Model.Commands.Core;
using TeamCloud.Model.Data;

namespace TeamCloud.Model.Commands
{
    public sealed class ComponentClearCommand : CustomCommand<Component, ComponentClearCommandResult>
    {
        public ComponentClearCommand(User user, Component payload) : base(user, payload)
        { }
    }
}
