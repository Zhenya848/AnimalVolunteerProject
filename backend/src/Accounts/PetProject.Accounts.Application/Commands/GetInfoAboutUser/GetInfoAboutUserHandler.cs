using CSharpFunctionalExtensions;
using PetProject.Accounts.Application.Repositories;
using PetProject.Accounts.Domain.User;
using PetProject.Core;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Repositories;
using PetProject.Core.ValueObjects.Dtos;
using PetProject.Core.ValueObjects.Dtos.ForQuery;
using PetProject.Core.ValueObjects.Dtos.ForQuery.Accounts;

namespace PetProject.Accounts.Application.Commands.GetInfoAboutUser;

public class GetInfoAboutUserHandler : ICommandHandler<Guid, Result<UserDto, ErrorList>>
{
    private readonly IAccountRepository _accountRepository;

    public GetInfoAboutUserHandler(
        IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<Result<UserDto, ErrorList>> Handle(Guid command, CancellationToken cancellationToken = default)
    {
        var userResult = await _accountRepository.GetInfoAboutUser(command, cancellationToken);
        
        if (userResult.IsFailure)
            return (ErrorList)userResult.Error;

        var participant = userResult.Value.ParticipantAccount;
        var volunteer = userResult.Value.VolunteerAccount;
        var admin = userResult.Value.AdminAccount;
        
        var participantAccount = participant is null ? null : new ParticipantAccountDto()
        { 
            FirstName = participant.FullName.FirstName, 
            LastName = participant.FullName.LastName,
            Patronymic = participant.FullName.Patronymic,
        };
        
        var volunteerAccount = volunteer is null ? null : new VolunteerAccountDto()
        { 
            FirstName = volunteer.FullName.FirstName, 
            LastName = volunteer.FullName.LastName,
            Patronymic = volunteer.FullName.Patronymic,
            
            Expirience = volunteer.Expirience,
            
            Requisites = volunteer.Requisites
                .Select(r => new RequisiteDto(r.Name, r.Description)).ToArray()
        };
        
        var adminAccount = admin is null ? null : new AdminAccountDto()
        { 
            FirstName = admin.FullName.FirstName, 
            LastName = admin.FullName.LastName,
            Patronymic = admin.FullName.Patronymic,
        };
        
        var user = new UserDto()
        {
            Id = userResult.Value.Id,
            
            UserName = userResult.Value.UserName!, 
            Email = userResult.Value.Email!,
            
            SocialNetworks = userResult.Value.SocialNetworks
                .Select(sn => new SocialNetworkDto(sn.Name, sn.Reference)).ToArray(),
            
            ParticipantAccount = participantAccount,
            VolunteerAccount = volunteerAccount,
            AdminAccount = adminAccount
        };

        return user;
    }
}