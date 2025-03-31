using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class UserHandler : AuthorizationHandler<UserRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, UserRequirement requirement)
    {
        var role = context.User.FindFirst(
            c => c.Type == ClaimTypes.Role);

        if (role is null)
        {
            return Task.CompletedTask;
        }

        var roleStr = Convert.ToString(role.Value);
        if (roleStr == requirement.Role)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}