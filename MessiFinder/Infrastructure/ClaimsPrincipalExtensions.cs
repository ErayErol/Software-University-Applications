namespace MessiFinder.Infrastructure
{
    using System.Security.Claims;

    using static Areas.Manager.ManagerConstants;

    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static bool IsManager(this ClaimsPrincipal user)
            => user.IsInRole(ManagerRoleName);
    }
}