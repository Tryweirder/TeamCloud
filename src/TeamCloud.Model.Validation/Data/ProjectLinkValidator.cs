﻿using FluentValidation;
using TeamCloud.Model.Data;

namespace TeamCloud.Model.Validation.Data
{
    public sealed class ProjectLinkValidator : AbstractValidator<ProjectLink>
    {
        public ProjectLinkValidator()
        {
            RuleFor(obj => obj.Id)
                .MustBeGuid();

            RuleFor(obj => obj.ProjectId)
                .MustBeGuid();

            RuleFor(obj => obj.HRef)
                .MustBeUrl();
        }
    }
}
