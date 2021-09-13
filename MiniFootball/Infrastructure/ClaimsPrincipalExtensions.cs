namespace MiniFootball.Infrastructure
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static bool IsManager(this ClaimsPrincipal user)
            => user.IsInRole(GlobalConstants.Manager.ManagerRoleName);
    }
}