namespace MiniFootball.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;

    public static class EndpointRouteBuilderExtensions
    {
        public static void MapDefaultRoute(this IEndpointRouteBuilder endpoints)
        {
            var name = "Admin Become";
            var pattern = "Admin/Admins/Become";
            var controller = "Admins";
            var action = "Become";
            AddRoute(endpoints, name, pattern, controller, action);

            name = "Game Details";
            pattern = "/Games/Details/{gameId}/{information}";
            controller = "Games";
            action = "Details";
            AddRoute(endpoints, name, pattern, controller, action);

            name = "Game Delete";
            pattern = "/Games/Delete/{gameId}/{information}";
            controller = "Games";
            action = "Delete";
            AddRoute(endpoints, name, pattern, controller, action);

            name = "Game Edit";
            pattern = "/Games/Edit/{gameId}/{information}";
            controller = "Games";
            action = "Edit";
            AddRoute(endpoints, name, pattern, controller, action);

            name = "Field Details";
            pattern = "/Fields/Details/{id}/{information}";
            controller = "Fields";
            action = "Details";
            AddRoute(endpoints, name, pattern, controller, action);

            name = "Field Edit";
            pattern = "/Fields/Edit/{id}/{information}";
            controller = "Fields";
            action = "Edit";
            AddRoute(endpoints, name, pattern, controller, action);

            name = "Field Delete";
            pattern = "/Fields/Delete/{id}/{information}";
            controller = "Fields";
            action = "Delete";
            AddRoute(endpoints, name, pattern, controller, action);
        }

        public static void MapDefaultAreaRoute(this IEndpointRouteBuilder endpoints)
        {
            var name = "Areas";
            var pattern = "{area:exists}/{controller=Home}/{action=Index}/{id?}";
            AddRouteArea(endpoints, name, pattern);
        }

        private static void AddRouteArea(IEndpointRouteBuilder endpoints, string name, string pattern)
        {
            endpoints
                .MapControllerRoute(
                    name: name,
                    pattern: pattern);
        }

        private static void AddRoute(
            IEndpointRouteBuilder endpoints, 
            string name, 
            string pattern, 
            string controller,
            string action)
        {
            endpoints
                .MapControllerRoute(
                    name: name,
                    pattern: pattern,
                    defaults: new
                    {
                        controller,
                        action
                    });
        }
    }
}
