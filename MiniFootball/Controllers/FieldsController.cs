namespace MiniFootball.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Fields;
    using Services.Countries;
    using Services.Fields;

    using static WebConstants;

    public class FieldsController : Controller
    {
        private readonly ICountryService country;
        private readonly IFieldService field;

        public FieldsController(
            ICountryService country,
            IFieldService field)
        {
            this.country = country;
            this.field = field;
        }

        [Authorize]
        public IActionResult Create()
        {
            if (this.User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = "Only Manager can create fields!";
                return View();
            }

            return View(new FieldCreateFormModel
            {
                Countries = this.country.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(FieldCreateFormModel fieldModel)
        {
            if (this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                fieldModel.Countries = this.country.All();
                return View(fieldModel);
            }

            if (this.field.IsSame(
                    fieldModel.Name,
                    fieldModel.Country,
                    fieldModel.Town))
            {
                fieldModel.Countries = this.country.All();
                TempData[GlobalMessageKey] = "There are already exist field with this Name, Country and Town";
                return View(fieldModel);
            }

            this.field.Create(
                fieldModel.Name,
                fieldModel.Country,
                fieldModel.Town,
                fieldModel.Address,
                fieldModel.ImageUrl,
                fieldModel.PhoneNumber,
                fieldModel.Parking,
                fieldModel.Cafe,
                fieldModel.Shower,
                fieldModel.ChangingRoom,
                fieldModel.Description);

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] FieldAllQueryModel query)
        {
            var queryResult = this.field.All(
                query.Town,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.PlaygroundsPerPage);

            var towns = this.field.Towns();

            query.TotalFields = queryResult.TotalFields;
            query.Fields = queryResult.Fields;
            query.Towns = towns;

            return View(query);
        }
    }
}
