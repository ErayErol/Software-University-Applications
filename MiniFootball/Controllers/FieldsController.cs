namespace MiniFootball.Controllers
{
    using System.Linq;
    using AutoMapper;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Fields;
    using Services.Admins;
    using Services.Cities;
    using Services.Countries;
    using Services.Fields;

    using static MyWebCase;
    using static WebConstants;

    public class FieldsController : Controller
    {
        private readonly ICountryService countries;
        private readonly ICityService cities;
        private readonly IAdminService admins;
        private readonly IFieldService fields;
        private readonly IMapper mapper;

        public FieldsController(
            ICountryService countries,
            IFieldService fields, 
            ICityService cities, 
            IMapper mapper, 
            IAdminService admins)
        {
            this.countries = countries;
            this.fields = fields;
            this.cities = cities;
            this.mapper = mapper;
            this.admins = admins;
        }

        [Authorize]
        public IActionResult Create()
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                TempData[GlobalMessageKey] = "Only Admins can create fields!";
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            return View(new FieldFormModel
            {
                Countries = this.countries.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(FieldFormModel fieldModel)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 || this.User.IsManager())
            {
                TempData[GlobalMessageKey] = "Only Admins can create fields!";
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                fieldModel.Countries = this.countries.All();
                return View(fieldModel);
            }

            fieldModel.Country = this.countries.Country(fieldModel.CountryName);
            fieldModel.City = this.cities.City(fieldModel.CityName);

            if (fieldModel.City == null)
            {
                RedirectToAction("Create", "Cities");
            }

            if (this.fields.IsExist(
                    fieldModel.Name,
                    fieldModel.Country.Id,
                    fieldModel.City.Id))
            {
                fieldModel.Countries = this.countries.All();
                TempData[GlobalMessageKey] = "There are already exist fields with this Name, Country and City";
                return View(fieldModel);
            }

            fieldModel.Name = FirstLetterUpperThenLower(fieldModel.Name);
            fieldModel.CountryName = FirstLetterUpperThenLower(fieldModel.CountryName);
            fieldModel.CityName = FirstLetterUpperThenLower(fieldModel.CityName);
            fieldModel.Address = FirstLetterUpperThenLower(fieldModel.Address);
            fieldModel.Description = FirstLetterUpperThenLower(fieldModel.Description);

            this.fields.Create(
                fieldModel.Name,
                fieldModel.Country.Id,
                fieldModel.City.Id,
                fieldModel.Address,
                fieldModel.ImageUrl,
                fieldModel.PhoneNumber,
                fieldModel.Parking,
                fieldModel.Cafe,
                fieldModel.Shower,
                fieldModel.ChangingRoom,
                fieldModel.Description,
                adminId);

            //TempData[GlobalMessageKey] = "You created game!";
            //return Redirect($"Details?id={id}");
            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] FieldAllQueryModel query)
        {
            var queryResult = this.fields.All(
                query.City,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.PlaygroundsPerPage);

            var queryCities = this.fields.Cities();

            query.TotalFields = queryResult.TotalFields;
            query.Fields = queryResult.Fields;
            query.Cities = queryCities;

            return View(query);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var field = this.fields.GetDetails(id);

            if (field == null)
            {
                return BadRequest();
            }

            return View(field);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.Id();

            if (this.admins.IsAdmin(userId) == false && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var field = this.fields.GetDetails(id);

            // TODO: Only creator of field can edit his own fields, not other fields, and others can not edit his fields
            //if (field?.UserId != userId)
            //{
            //    return Unauthorized();
            //}

            var fieldForm = this.mapper.Map<FieldFormModel>(field);

            return View(fieldForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(FieldFormModel fieldModel)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                return View(fieldModel);
            }

            if (this.fields.IsByAdmin(fieldModel.Id, adminId) == false && this.User.IsManager() == false)
            {
                return BadRequest();
            }

            var isEdit = this.fields.Edit(
                fieldModel.Id,
                fieldModel.Name,
                fieldModel.Address,
                fieldModel.ImageUrl,
                fieldModel.Parking,
                fieldModel.Shower,
                fieldModel.ChangingRoom,
                fieldModel.Cafe,
                fieldModel.Description);

            if (isEdit == false)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You edited field!";
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.Id();

            if (this.admins.IsAdmin(userId) == false && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var field = this.fields.GetDetails(id);

            if (field == null)
            {
                return BadRequest();
            }

            return View(field);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(FieldDetailServiceModel fieldDetails)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (this.fields.Delete(fieldDetails.Id) == false)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You deleted field!";
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Mine(int id)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var myGames = this.fields
                .ByUser(this.User.Id())
                .OrderByDescending(f => f.Id);

            return View(myGames);
        }
    }
}
