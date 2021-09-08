namespace MiniFootball.Controllers
{
    using Areas.Admin.Controllers;
    using AspNetCoreHero.ToastNotification.Abstractions;
    using AutoMapper;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Models.Fields;
    using Services.Admins;
    using Services.Cities;
    using Services.Countries;
    using Services.Fields;
    using System;
    using System.IO;
    using System.Linq;
    
    using static Convert;
    using static GlobalConstant;
    using static GlobalConstant.Error;

    public class FieldsController : Controller
    {
        private readonly IFieldService fields;
        private readonly IAdminService admins;
        private readonly ICountryService countries;
        private readonly ICityService cities;
        private readonly IMapper mapper;
        private readonly INotyfService notifications;
        private readonly IWebHostEnvironment hostEnvironment;

        public FieldsController(IFieldService fields,
                                IAdminService admins,
                                ICountryService countries,
                                ICityService cities,
                                IMapper mapper,
                                INotyfService notifications,
                                IWebHostEnvironment hostEnvironment)
        {
            this.fields = fields;
            this.admins = admins;
            this.countries = countries;
            this.cities = cities;
            this.mapper = mapper;
            this.notifications = notifications;
            this.hostEnvironment = hostEnvironment;
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

            if (query.Fields.Any() == false)
            {
                notifications.Information(Field.DoNotHaveAnyFieldsYet);
            }

            return View(query);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                notifications.Error(Field.OnlyAdminCanCreate);
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var createFormModel = new FieldFormServiceModel
            {
                Countries = countries.All()
            };

            return View(createFormModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(FieldFormServiceModel fieldServiceModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 || User.IsManager())
            {
                notifications.Error(Field.OnlyAdminCanCreate);
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            if (ModelState.IsValid == false)
            {
                fieldServiceModel.Countries = countries.All();
                return View(fieldServiceModel);
            }

            fieldServiceModel.CountryId = countries.CountryIdByName(fieldServiceModel.CountryName);
            fieldServiceModel.CityId = cities.CityIdByName(fieldServiceModel.CityName);

            if (fieldServiceModel.CityId == 0)
            {
                notifications.Error(City.CityDoesNotExistInCountry);
                return RedirectToAction(GlobalConstant.Create, City.ControllerName);
            }

            if (fields.IsAlreadyExist(fieldServiceModel.Name, fieldServiceModel.CountryId, fieldServiceModel.CityId))
            {
                fieldServiceModel.Countries = countries.All();
                notifications.Error(Field.ThereAreAlreadyExistField);
                return View(fieldServiceModel);
            }

            fieldServiceModel.CountryName = ToTitleCase(fieldServiceModel.CountryName);
            fieldServiceModel.CityName = ToTitleCase(fieldServiceModel.CityName);

            fieldServiceModel.PhotoPath = ProcessUploadFile(fieldServiceModel);

            var fieldId = FieldId(fieldServiceModel, adminId);

            if (fieldId == 0)
            {
                notifications.Error(SomethingIsWrong);
                return RedirectToAction(Name, Name);
            }

            notifications.Success(Field.SuccessfullyCreated);
            return RedirectToAction(nameof(Mine));
        }

        private int FieldId(FieldFormServiceModel fieldServiceModel, int adminId)
            => fields.Create(fieldServiceModel.Name,
                             fieldServiceModel.CountryId,
                             fieldServiceModel.CityId,
                             fieldServiceModel.Address,
                             fieldServiceModel.PhotoPath,
                             fieldServiceModel.PhoneNumber,
                             fieldServiceModel.Parking,
                             fieldServiceModel.Cafe,
                             fieldServiceModel.Shower,
                             fieldServiceModel.ChangingRoom,
                             fieldServiceModel.Description,
                             adminId);

        [Authorize]
        public IActionResult Edit(int id, string information)
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false && User.IsManager() == false)
            {
                notifications.Error(Field.OnlyCreatorCanEdit);
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var fieldModel = fields.Details(id);

            if (fieldModel.Name.Equals(information) == false)
            {
                notifications.Error(SomethingIsWrong);
                return RedirectToAction(Name, Name);
            }

            return View(fieldModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(FieldFormServiceModel fieldModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 && User.IsManager() == false)
            {
                notifications.Error(Field.OnlyCreatorCanEdit);
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            if (ModelState.IsValid == false)
            {
                return View(fieldModel);
            }

            if (fields.IsAdminCreatorOfField(fieldModel.Id, adminId) == false && User.IsManager() == false)
            {
                notifications.Error(Field.OnlyCreatorCanEdit);
                return RedirectToAction(Name, Name);
            }

            if (fieldModel.Photo != null)
            {
                fieldModel.PhotoPath = ProcessUploadFile(fieldModel);
            }

            if (IsEdit(fieldModel) == false)
            {
                notifications.Error(SomethingIsWrong);
                return RedirectToAction(Name, Name);
            }

            notifications.Information(
                "Your field was edited" +
                $"{(User.IsManager() ? string.Empty : " and is awaiting approval")}!");

            var routeValues = new
            {
                id = fieldModel.Id,
                information = fields.FieldName(fieldModel.Id)
            };

            return RedirectToAction(nameof(Details), routeValues);
        }

        private bool IsEdit(FieldDetailServiceModel fieldModel)
            => fields.Edit(fieldModel.Id,
                           fieldModel.Name,
                           fieldModel.Address,
                           fieldModel.PhotoPath,
                           fieldModel.Parking,
                           fieldModel.Shower,
                           fieldModel.ChangingRoom,
                           fieldModel.Cafe,
                           fieldModel.Description,
                           fieldModel.PhoneNumber,
                           User.IsManager());

        [Authorize]
        public IActionResult Delete(int id, string information)
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false && User.IsManager() == false)
            {
                notifications.Error(Field.OnlyCreatorCanDelete);
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var field = fields.FieldExist(id);

            if (field == false)
            {
                notifications.Error(Field.DoesNotExist);
                return RedirectToAction(Name, Name);
            }

            var fieldDeleteDetails = fields.FieldDeleteInfo(id);

            if (fieldDeleteDetails.Name.Equals(information) == false)
            {
                notifications.Error(SomethingIsWrong);
                return RedirectToAction(Name, Name);
            }

            return View(fieldDeleteDetails);
        }

        [HttpPost]
        [Authorize]
        public RedirectToActionResult Delete(FieldDeleteServiceModel fieldModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 && User.IsManager() == false)
            {
                notifications.Error(Field.OnlyCreatorCanDelete);
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            if (fields.IsAdminCreatorOfField(fieldModel.Id, adminId) == false && User.IsManager() == false)
            {
                notifications.Error(Field.OnlyCreatorCanDelete);
                return RedirectToAction(Name, Name);
            }

            if (fields.Delete(fieldModel.Id) == false)
            {
                notifications.Error(SomethingIsWrong);
                return RedirectToAction(Name, Name);
            }

            notifications.Warning(Field.SuccessfullyDelete);
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Details(int id, string information)
        {
            var fieldModel = mapper.Map<FieldDetailServiceModel>(fields.Details(id));
            if (fieldModel == null || fieldModel.Name.Equals(information) == false)
            {
                notifications.Error(SomethingIsWrong);
                return RedirectToAction(Name, Name);
            }

            return View(fieldModel);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false || User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var myFields = fields
                .FieldsWhereCreatorIsUser(userId)
                .OrderByDescending(f => f.Id);

            if (myFields.Any() == false)
            {
                notifications.Error(Field.YouDoNotHaveAnyFieldsYet);
            }

            return View(myFields);
        }

        private string ProcessUploadFile(FieldDetailServiceModel fieldModel)
        {
            string uniqueFileName = null;

            if (fieldModel.Photo != null)
            {
                var uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + fieldModel.Photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                fieldModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return uniqueFileName;
        }
    }
}