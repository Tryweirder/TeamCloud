/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System;
using FluentValidation;
using TeamCloud.Model.Data;

namespace TeamCloud.Model.Validation.Data
{
    public sealed class ComponentValidator : AbstractValidator<Component>
    {
        public ComponentValidator()
        {
            RuleFor(obj => obj.Id)
                .MustBeGuid();

            RuleFor(obj => obj.ProjectId)
                .MustBeGuid();

            RuleFor(obj => obj.ProviderId)
                .MustBeProviderId();

            RuleFor(obj => obj.RequestedBy)
                .MustBeGuid();
        }
    }
}
