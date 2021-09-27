namespace MiniFootball.Areas.Manager.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static GlobalConstants;

    [Area(Manager.AreaName)]
    [Authorize(Roles = Manager.ManagerRoleName)]
    public abstract class ManagerController : Controller
    {
    }
}
