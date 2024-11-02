using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Extensions;
using PetProject.Core.Application.Repositories;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Volunteers.Application.Volunteers.Queries
{
    public class GetVolunteersWithPaginationHandler : IQueryHandler<GetVolunteersWithPaginationQuery, 
        Result<PagedList<VolunteerDto>, ErrorList>>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly IValidator<GetVolunteersWithPaginationQuery> _validator;

        public GetVolunteersWithPaginationHandler(IReadDbContext readDbContext, 
            IValidator<GetVolunteersWithPaginationQuery> validator)
        {
            _readDbContext = readDbContext;
            _validator = validator;
        }

        public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Handle(
            GetVolunteersWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();
            
            var volunteersQuery = _readDbContext.Volunteers;

            Expression<Func<VolunteerDto, object>> selector = query.OrderBy?.ToLower() switch
            {
                "first_name" => volunteer => volunteer.FirstName,
                "last_name" => volunteer => volunteer.LastName,
                "patronymic" => volunteer => volunteer.Patronymic,

                "description" => volunteer => volunteer.Description,
                "experience" => volunteer => volunteer.Experience,
                _ => volunteer => volunteer.Id
            };

            volunteersQuery = query.OrderByDesc 
                ? volunteersQuery.OrderByDescending(selector) : volunteersQuery.OrderBy(selector);

            return await volunteersQuery.GetItemsWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
