using PetProject.Core.ValueObjects;

namespace PetProject.Accounts.Domain.User;

public class AdminAccount
{
    public const string ADMIN = "Admin";
    
    public Guid Id { get; set; }
    public FullName FullName { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }

    private AdminAccount()
    {
        
    }
    
    public AdminAccount(FullName fullName, User user)
    {
        Id = Guid.NewGuid();
        FullName = fullName;

        User = user;
    }
}