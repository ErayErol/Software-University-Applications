namespace MiniFootball.Controllers
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    using static GlobalConstant.Error;

    public class ErrorController : Controller
    {
        [Route(ErrorRoute)]
        public IActionResult Error(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            
            if (statusCode != 0)
            {
                ViewBag.StatusCode = statusCode;
                ViewBag.Path = statusCodeResult.OriginalPath;
                ViewBag.QS = statusCodeResult.OriginalQueryString;

                if (statusCode == 404)
                {
                    ViewBag.ErrorMessage = ErrorMessage404;
                }
            }

            return View(Name);
        }
    }
}
