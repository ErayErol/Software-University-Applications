namespace MiniFootball.Areas.Manager.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Services.Fields;

    [Area(ManagerConstants.AreaName)]
    public class FieldsController : ManagerController
    {
        private readonly IFieldService fields;

        public FieldsController(IFieldService fields)
        {
            this.fields = fields;
        }

        public IActionResult AllFields()
        {
            var fields = this.fields
                .All(publicOnly: false)
                .Fields
                .OrderByDescending(f => f.Id);

            return View(fields);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.fields.ChangeVisibility(id);

            return RedirectToAction(nameof(AllFields));
        }
    }
}
