using PetProject.Core.ValueObjects;

namespace PetProject.Accounts.Domain.User;

public class ParticipantAccount
{
    public const string PARTICIPANT = "Participant";
    
    public Guid Id { get; set; }
    public FullName FullName { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }

    private ParticipantAccount()
    {
        
    }
    
    public static ParticipantAccount CreateParticipant(FullName fullName, User user)
    {
        return new ParticipantAccount()
        {
            Id = Guid.NewGuid(),
            FullName = fullName,

            User = user
        };
    }
}