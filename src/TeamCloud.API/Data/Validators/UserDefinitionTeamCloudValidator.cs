﻿/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using FluentValidation;
using TeamCloud.Model.Validation;

namespace TeamCloud.API.Data.Validators
{
    public sealed class UserDefinitionTeamCloudValidator : AbstractValidator<UserDefinition>
    {
        public UserDefinitionTeamCloudValidator()
        {
            RuleFor(obj => obj.Identifier).MustBeUserIdentifier();
            RuleFor(obj => obj.Role).MustBeTeamCloudUserRole();
        }
    }
}
