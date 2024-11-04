using PetProject.Core.ValueObjects;

namespace PetProject.Accounts.Domain.User;

public class VolunteerAccount
{
    public const string VOLUNTEER = "Volunteer";
    
    public Guid Id { get; set; }
    public FullName FullName { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public int Expirience { get; set; }
    
    private List<Requisite> _requisites = [];
    public IReadOnlyList<Requisite> Requisites => _requisites;

    private VolunteerAccount()
    {
        
    }
    
    public static VolunteerAccount CreateVolunteer(
        FullName fullName, 
        User user,
        int expirience,
        IEnumerable<Requisite> requisites)
    {
        return new VolunteerAccount()
        {
            Id = Guid.NewGuid(),
            FullName = fullName,

            User = user,
            
            Expirience = expirience,
            _requisites = requisites.ToList()
        };
    }
}