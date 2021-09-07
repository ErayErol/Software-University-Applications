namespace MiniFootball.Controllers
{
    using System;
    using System.IO;
    using Areas.Admin.Controllers;
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Fields;
    using Services.Admins;
    using Services.Cities;
    using Services.Countries;
    using Services.Fields;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    using static Convert;
    using static GlobalConstant;
    using static GlobalConstant.Error;

    public class FieldsController : Controller
    {
        private readonly IFieldService fields;
        private readonly IAdminService admins;
        private readonly ICountryService countries;
        private readonly ICityService cities;
        private readonly INotyfService notifications;
        private readonly IWebHostEnvironment hostEnvironment;

        public FieldsController(IFieldService fields,
                                IAdminService admins,
                                ICountryService countries,
                                ICityService cities,
                                INotyfService notifications,
                                IWebHostEnvironment hostEnvironment)
        {
            this.fields = fields;
            this.admins = admins;
            this.countries = countries;
            this.cities = cities;
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

            var createFormModel = new FieldFormModel
            {
                Countries = countries.All()
            };

            return View(createFormModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(FieldFormModel fieldModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 || User.IsManager())
            {
                notifications.Error(Field.OnlyAdminCanCreate);
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
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
                notifications.Error(City.CityDoesNotExistInCountry);
                return RedirectToAction(GlobalConstant.Create, City.ControllerName);
            }

            if (fields.IsAlreadyExist(fieldModel.Name, fieldModel.CountryId, fieldModel.CityId))
            {
                fieldModel.Countries = countries.All();
                notifications.Error(Field.ThereAreAlreadyExistField);
                return View(fieldModel);
            }

            fieldModel.CountryName = ToTitleCase(fieldModel.CountryName);
            fieldModel.CityName = ToTitleCase(fieldModel.CityName);

            var uniqueFileName = ProcessUploadFile(fieldModel);

            var fieldId = FieldId(fieldModel, uniqueFileName, adminId);

            if (fieldId == 0)
            {
                notifications.Error(SomethingIsWrong);
                return RedirectToAction(Name, Name);
            }

            notifications.Success(Field.SuccessfullyCreated);
            return RedirectToAction(nameof(Mine));
        }

        private int FieldId(FieldFormModel fieldModel,
                            string uniqueFileName,
                            int adminId)
            => fields.Create(fieldModel.Name,
                             fieldModel.CountryId,
                             fieldModel.CityId,
                             fieldModel.Address,
                             uniqueFileName,
                             fieldModel.PhoneNumber,
                             fieldModel.Parking,
                             fieldModel.Cafe,
                             fieldModel.Shower,
                             fieldModel.ChangingRoom,
                             fieldModel.Description,
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

            var fieldDetails = fields.GetDetails(id);

            if (fieldDetails.Name.Equals(information) == false)
            {
                notifications.Error(SomethingIsWrong);
                return RedirectToAction(Name, Name);
            }

            return View(fieldDetails);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(FieldDetailServiceModel fieldModel)
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

            var isEdit = IsEdit(fieldModel, fieldModel.PhotoPath);

            if (isEdit == false)
            {
                notifications.Error(SomethingIsWrong);
                return RedirectToAction(Name, Name);
            }

            notifications.Information(
                "Your field was edited" +
                $"{(User.IsManager() ? string.Empty : " and is awaiting approval")}!");

            var fieldName = fields.FieldName(fieldModel.Id);

            var routeValues = new
            {
                id = fieldModel.Id,
                information = fieldName
            };

            return RedirectToAction(nameof(Details), routeValues);
        }

        private string ProcessUploadFile(FieldDetailServiceModel fieldModel)
        {
            string uniqueFileName = null;
            if (fieldModel.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + fieldModel.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                fieldModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return uniqueFileName;
        }

        private bool IsEdit(FieldDetailServiceModel fieldModel, string uniqueFileName)
            => fields.Edit(fieldModel.Id,
                           fieldModel.Name,
                           fieldModel.Address,
                           uniqueFileName,
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
            var fieldDetails = fields.GetDetails(id);

            if (fieldDetails == null || fieldDetails.Name.Equals(information) == false)
            {
                notifications.Error(SomethingIsWrong);
                return RedirectToAction(Name, Name);
            }

            return View(fieldDetails);
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
    }
}