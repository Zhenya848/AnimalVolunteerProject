﻿namespace PetProject.Volunteers.Application.Pets.Queries
{
    public record GetPetsWithPaginationQuery(
        int Page, 
        int PageSize,
        int? PositionFrom = null,
        int? PositionTo = null,
        bool OrderByDesc = false,
        string? OrderBy = null);
}
