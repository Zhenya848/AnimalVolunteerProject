namespace PetProject.Volunteers.Application.Files.Commands.Create
{
    public record CreateFilesCommand(IEnumerable<FileData> Files, string BucketName);
}
