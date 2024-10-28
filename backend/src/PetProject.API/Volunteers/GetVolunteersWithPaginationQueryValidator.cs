using System.Runtime.InteropServices.JavaScript;
using FluentValidation;
using PetProject.Application.Volunteers.Queries;
using PetProject.Domain.Shared;

namespace PetProject.API.Volunteers;

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