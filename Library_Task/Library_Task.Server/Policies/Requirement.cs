using Microsoft.AspNetCore.Authorization;

public class UserRequirement : IAuthorizationRequirement
{
    public UserRequirement(string role) =>
        Role = role;

    public string Role { get; }
}