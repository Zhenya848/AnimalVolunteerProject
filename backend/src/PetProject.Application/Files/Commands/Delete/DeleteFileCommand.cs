namespace PetProject.Application.Files.Commands.Delete
{
    public record DeleteFileCommand(string BucketName, string ObjectName);
}
