using FluentValidation;
using PetProject.Core;
using PetProject.Core.Application.Validation;
using PetProject.Volunteers.Application.Volunteers.Queries;

namespace PetProject.Volunteers.Application.Validators;

public class GetVolunteersWithPaginationQueryValidator : AbstractValidator<GetVolunteersWithPaginationQuery>
{
    public GetVolunteersWithPaginationQueryValidator()
    {
        RuleFor(q => q.Page).GreaterThan(0)
            .WithError(Error.Validation("page.is.invalid", "Page must be greater than 0"));
        
        RuleFor(q => q.PageSize).GreaterThan(0)
            .WithError(Error.Validation("page_size.is.invalid", "Page size must be greater than 0"));
    }
}