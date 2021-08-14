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

    using static Convert;
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
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                TempData[GlobalMessageKey] = "Only Admins can create fields!";
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            return View(new FieldFormModel
            {
                Countries = countries.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(FieldFormModel fieldModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 || User.IsManager())
            {
                TempData[GlobalMessageKey] = "Only Admins can create fields!";
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                fieldModel.Countries = countries.All();
                return View(fieldModel);
            }

            fieldModel.Country = countries.Country(fieldModel.CountryName);
            fieldModel.City = cities.City(fieldModel.CityName);

            if (fieldModel.City == null)
            {
                RedirectToAction("Create", "Cities");
            }

            if (fields.IsExist(fieldModel.Name, fieldModel.Country.Id, fieldModel.City.Id))
            {
                fieldModel.Countries = countries.All();
                TempData[GlobalMessageKey] = "There are already exist fields with this Name, Country and City";
                return View(fieldModel);
            }

            fieldModel.Name = ToTitleCase(fieldModel.Name);
            fieldModel.CountryName = ToTitleCase(fieldModel.CountryName);
            fieldModel.CityName = ToTitleCase(fieldModel.CityName);
            fieldModel.Address = ToTitleCase(fieldModel.Address);
            fieldModel.Description = ToTitleCase(fieldModel.Description);

            var fieldId = fields.Create(
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

            if (fieldId == 0)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You created field!";
            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] FieldAllQueryModel query)
        {
            var queryResult = fields.All(
                query.City,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.PlaygroundsPerPage);

            var queryCities = fields.Cities();

            query.TotalFields = queryResult.TotalFields;
            query.Fields = queryResult.Fields;
            query.Cities = queryCities;

            return View(query);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var field = fields.GetDetails(id);

            if (field == null)
            {
                return BadRequest();
            }

            return View(field);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false && User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var field = fields.GetDetails(id);

            var fieldForm = mapper.Map<FieldFormModel>(field);

            return View(fieldForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(FieldFormModel fieldModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 && User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                return View(fieldModel);
            }

            if (fields.IsByAdmin(fieldModel.Id, adminId) == false && User.IsManager() == false)
            {
                return BadRequest();
            }

            var isEdit = fields.Edit(
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
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false && User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var field = fields.GetDetails(id);

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
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 && User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (fields.IsByAdmin(fieldDetails.Id, adminId) == false && User.IsManager() == false)
            {
                return BadRequest();
            }

            if (fields.Delete(fieldDetails.Id) == false)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You deleted field!";
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Mine(int id)
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false || User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var myFields = fields
                .ByUser(userId)
                .OrderByDescending(f => f.Id);

            if (myFields.Any() == false)
            {
                TempData[GlobalMessageKey] = "Still you do not have fields, but you can create it!";
            }

            return View(myFields);
        }
    }
}
