namespace PetProject.Framework.Authorization;

public static class Permissions
{
    public static class Accounts
    {
        public const string Admin = "admin.check";
    }
    
    public static class Volunteers
    {
        public static string CreateVolunteer = "volunteer.create";
        public static string ReadVolunteer = "volunteer.read";
        public static string UpdateVolunteer = "volunteer.update";
        public static string DeleteVolunteer = "volunteer.delete";
    }
}