namespace MiniFootball.Areas.Manager.Controller
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static ManagerConstants;

    [Area(AreaName)]
    [Authorize(Roles = ManagerRoleName)]
    public abstract class ManagerController : Controller
    {
    }
}
