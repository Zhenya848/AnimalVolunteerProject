using CSharpFunctionalExtensions;
using PetProject.Accounts.Application.Repositories;
using PetProject.Accounts.Domain.User;
using PetProject.Core;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.ValueObjects.Dtos;
using PetProject.Core.ValueObjects.Dtos.ForQuery;
using PetProject.Core.ValueObjects.Dtos.ForQuery.Accounts;

namespace PetProject.Accounts.Application.Commands.GetUsers;

public class GetUsersHandler : ICommandHandler<GetUsersCommand, IEnumerable<UserDto>>
{
    private readonly IAccountRepository _accountRepository;

    public GetUsersHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<IEnumerable<UserDto>> Handle(
        GetUsersCommand command, 
        CancellationToken cancellationToken = default)
    {
        var users= await _accountRepository
            .GetUsers(command.Users, command.Roles, cancellationToken);
        
        var usersDto = users.Select(u => new UserDto()
        {
            Id = u.Id,

            UserName = u.UserName!,
            Email = u.Email!,
            
            SocialNetworks = u.SocialNetworks
                .Select(s => new SocialNetworkDto(s.Name, s.Reference)),

            AdminAccount = u.AdminAccount != null
                ? new AdminAccountDto()
                {
                    FirstName = u.AdminAccount.FullName.FirstName,
                    LastName = u.AdminAccount.FullName.LastName,
                    Patronymic = u.AdminAccount.FullName.Patronymic
                }
                : null,

            ParticipantAccount = u.ParticipantAccount != null
                ? new ParticipantAccountDto()
                {
                    FirstName = u.ParticipantAccount.FullName.FirstName,
                    LastName = u.ParticipantAccount.FullName.LastName,
                    Patronymic = u.ParticipantAccount.FullName.Patronymic
                }
                : null,

            VolunteerAccount = u.VolunteerAccount != null
                ? new VolunteerAccountDto()
                {
                    FirstName = u.VolunteerAccount.FullName.FirstName,
                    LastName = u.VolunteerAccount.FullName.LastName,
                    Patronymic = u.VolunteerAccount.FullName.Patronymic,

                    Requisites = u.VolunteerAccount.Requisites
                        .Select(r => new RequisiteDto(r.Name, r.Description))
                        .ToArray()
                } 
                : null
        });
        
        return usersDto;
    }
}