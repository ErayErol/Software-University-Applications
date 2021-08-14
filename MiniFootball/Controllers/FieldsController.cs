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

        public IActionResult All([FromQuery] FieldAllQueryModel query)
        {
            var queryResult = fields.All(
                query.City,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.PlaygroundsPerPage);

            var queryCities = fields.AllCreatedCitiesName();

            query.TotalFields = queryResult.TotalFields;
            query.Fields = queryResult.Fields;
            query.Cities = queryCities;

            return View(query);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                TempData[GlobalMessageKey] = "Only Admins can create fields!";
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            return View(new FieldCreateFormModel
            {
                Countries = countries.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(FieldCreateFormModel fieldCreateModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 || User.IsManager())
            {
                TempData[GlobalMessageKey] = "Only Admins can create fields!";
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                fieldCreateModel.Countries = countries.All();
                return View(fieldCreateModel);
            }

            fieldCreateModel.CountryId = countries.CountryIdByName(fieldCreateModel.CountryName);
            fieldCreateModel.CityId = cities.CityIdByName(fieldCreateModel.CityName);

            if (fieldCreateModel.CityId == 0)
            {
                RedirectToAction("Create", "Cities");
            }

            if (fields.IsAlreadyExist(fieldCreateModel.Name, fieldCreateModel.CountryId, fieldCreateModel.CityId))
            {
                fieldCreateModel.Countries = countries.All();
                TempData[GlobalMessageKey] = "There are already exist fields with this Name, Country and City";
                return View(fieldCreateModel);
            }

            fieldCreateModel.CountryName = ToTitleCase(fieldCreateModel.CountryName);
            fieldCreateModel.CityName = ToTitleCase(fieldCreateModel.CityName);

            var fieldId = fields.Create(
                fieldCreateModel.Name,
                fieldCreateModel.CountryId,
                fieldCreateModel.CityId,
                fieldCreateModel.Address,
                fieldCreateModel.ImageUrl,
                fieldCreateModel.PhoneNumber,
                fieldCreateModel.Parking,
                fieldCreateModel.Cafe,
                fieldCreateModel.Shower,
                fieldCreateModel.ChangingRoom,
                fieldCreateModel.Description,
                adminId);

            if (fieldId == 0)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You created field!";
            return RedirectToAction(nameof(All));
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

            var fieldForm = this.mapper.Map<FieldEditFormModel>(field);

            return View(fieldForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(FieldEditFormModel fieldCreateModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 && User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                return View(fieldCreateModel);
            }

            if (fields.IsAdminCreatorOfField(fieldCreateModel.Id, adminId) == false && User.IsManager() == false)
            {
                return BadRequest();
            }

            var isEdit = fields.Edit(
                fieldCreateModel.Id,
                fieldCreateModel.Name,
                fieldCreateModel.Address,
                fieldCreateModel.ImageUrl,
                fieldCreateModel.Parking,
                fieldCreateModel.Shower,
                fieldCreateModel.ChangingRoom,
                fieldCreateModel.Cafe,
                fieldCreateModel.Description,
                fieldCreateModel.PhoneNumber);

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

            var field = fields.FieldExist(id);

            if (field == false)
            {
                return BadRequest();
            }

            return View(new FieldDeleteServiceModel
            {
                Id = id
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(FieldDeleteServiceModel fieldModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 && User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (fields.IsAdminCreatorOfField(fieldModel.Id, adminId) == false && User.IsManager() == false)
            {
                return BadRequest();
            }

            if (fields.Delete(fieldModel.Id) == false)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You deleted field!";
            return RedirectToAction(nameof(All));
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
        public IActionResult Mine()
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false || User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var myFields = fields
                .FieldsWhereCreatorIsUser(userId)
                .OrderByDescending(f => f.Id);

            if (myFields.Any() == false)
            {
                TempData[GlobalMessageKey] = "Still you do not have fields, but you can create it!";
            }

            return View(myFields);
        }
    }
}
