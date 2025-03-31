namespace Library_Task.Server.DTO
{
    public record LogInModel(
    string Email,
    string Password,
    bool? IsAdmin);
}
