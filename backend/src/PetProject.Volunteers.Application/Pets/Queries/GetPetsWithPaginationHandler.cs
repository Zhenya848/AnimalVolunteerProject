using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Extensions;
using PetProject.Core.Application.Repositories;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Volunteers.Application.Pets.Queries
{
    public class GetPetsWithPaginationHandler : IQueryHandler<GetPetsWithPaginationQuery, 
        Result<PagedList<PetDto>, ErrorList>>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly IValidator<GetPetsWithPaginationQuery> _validator;

        public GetPetsWithPaginationHandler(IReadDbContext readDbContext, 
            IValidator<GetPetsWithPaginationQuery> validator)
        {
            _readDbContext = readDbContext;
            _validator = validator;
        }

        public async Task<Result<PagedList<PetDto>, ErrorList>> Handle(
            GetPetsWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ValidationErrorResponse();
            
            var petsQuery = _readDbContext.Pets;

            var speciesQuery = _readDbContext.Species;
            var breedsQuery = _readDbContext.Breeds;

            Expression<Func<PetDto, object>> selector = query.OrderBy?.ToLower() switch
            {
                "volunteerId" => pet => pet.VolunteerId,
                "name" => pet => pet.Name,
                "color" => pet => pet.Color,
                "species" => pet => speciesQuery.First(i => i.Id == pet.Id),
                "breeds" => pet => breedsQuery.First(i => i.Id == pet.Id),
                "description" => pet => pet.Description,
                "weight" => pet => pet.Weight,
                "height" => pet => pet.Height,
                "street" => pet => pet.Street,
                "city" => pet => pet.City,
                "state" => pet => pet.State,
                "zipcode" => pet => pet.Zipcode,

                _ => pet => pet.Id
            };

            /*
               public string Street { get; }
               public string City { get; }
               public string State { get; }
               public string ZipCode { get; }*/

            petsQuery = query.OrderByDesc
                ? petsQuery.OrderByDescending(selector) : petsQuery.OrderBy(selector);

            petsQuery = petsQuery
                .WhereIf(query.PositionFrom != null, sn => sn.SerialNumber >= query.PositionFrom);

            petsQuery = petsQuery
                .WhereIf(query.PositionTo != null, sn => sn.SerialNumber <= query.PositionTo);

            return await petsQuery.GetItemsWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
