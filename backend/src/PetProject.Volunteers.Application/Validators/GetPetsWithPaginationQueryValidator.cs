using FluentValidation;
using PetProject.Core;
using PetProject.Core.Application.Validation;
using PetProject.Volunteers.Application.Pets.Queries;

namespace PetProject.Volunteers.Application.Validators;

public class GetPetsWithPaginationQueryValidator : AbstractValidator<GetPetsWithPaginationQuery>
{
    public GetPetsWithPaginationQueryValidator()
    {
        RuleFor(q => q.Page).GreaterThan(0)
            .WithError(Error.Validation("page.is.invalid", "Page must be greater than 0"));
        
        RuleFor(q => q.PageSize).GreaterThan(0)
            .WithError(Error.Validation("page_size.is.invalid", "Page size must be greater than 0"));
    }
}