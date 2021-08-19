namespace MiniFootball.Controllers
{
    using AutoMapper;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Fields;
    using Services.Admins;
    using Services.Cities;
    using Services.Countries;
    using Services.Fields;
    using System.Linq;
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
        public IActionResult Create(FieldCreateFormModel fieldModel)
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

            fieldModel.CountryId = countries.CountryIdByName(fieldModel.CountryName);
            fieldModel.CityId = cities.CityIdByName(fieldModel.CityName);

            if (fieldModel.CityId == 0)
            {
                TempData[GlobalMessageKey] =
                    "This City does not exist in this Country. First you have to create City and then create Game!";
                return RedirectToAction("Create", "Cities");
            }

            if (fields.IsAlreadyExist(fieldModel.Name, fieldModel.CountryId, fieldModel.CityId))
            {
                fieldModel.Countries = countries.All();
                TempData[GlobalMessageKey] = "There are already exist fields with this Name, Country and City";
                return View(fieldModel);
            }

            fieldModel.CountryName = ToTitleCase(fieldModel.CountryName);
            fieldModel.CityName = ToTitleCase(fieldModel.CityName);

            var fieldId = fields.Create(
                fieldModel.Name,
                fieldModel.CountryId,
                fieldModel.CityId,
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

            TempData[GlobalMessageKey] = "Your field was created and is awaiting approval!";
            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public IActionResult Edit(int id, string information)
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false && User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var field = fields.GetDetails(id);

            if (field.Name.Equals(information) == false)
            {
                return BadRequest();
            }

            var fieldForm = mapper.Map<FieldEditFormModel>(field);

            return View(fieldForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(FieldEditFormModel fieldModel)
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

            if (fields.IsAdminCreatorOfField(fieldModel.Id, adminId) == false && User.IsManager() == false)
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
                fieldModel.Description,
                fieldModel.PhoneNumber,
                this.User.IsManager());

            if (isEdit == false)
            {
                return BadRequest();
            }

            var fieldName = fields.FieldName(fieldModel.Id);

            TempData[GlobalMessageKey] = $"Your field was edited{(this.User.IsManager() ? string.Empty : " and is awaiting approval")}!";
            return RedirectToAction(nameof(Details), new { id = fieldModel.Id, information = fieldName });
        }

        [Authorize]
        public IActionResult Delete(int id, string information)
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

            var fieldDeleteDetails = fields.FieldDeleteInfo(id);

            if (fieldDeleteDetails.Name.Equals(information) == false)
            {
                return BadRequest();
            }

            return View(fieldDeleteDetails);
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
        public IActionResult Details(int id, string information)
        {
            var fieldDetails = fields.GetDetails(id);

            if (fieldDetails == null || fieldDetails.Name.Equals(information) == false)
            {
                return BadRequest();
            }

            return View(fieldDetails);
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
