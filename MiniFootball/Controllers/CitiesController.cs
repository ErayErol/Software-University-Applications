namespace MiniFootball.Controllers
{
    using Areas.Admin.Controllers;
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cities;
    using Services.Admins;
    using Services.Cities;
    using Services.Countries;

    using static Convert;
    using static GlobalConstants;
    using static GlobalConstants.Notifications;

    [Authorize]
    public class CitiesController : Controller
    {
        private readonly ICityService cities;
        private readonly ICountryService countries;
        private readonly IAdminService admins;
        private readonly INotyfService notifications;

        public CitiesController(ICityService cities,
                                ICountryService countries,
                                IAdminService admins,
                                INotyfService notifications)
        {
            this.cities = cities;
            this.countries = countries;
            this.admins = admins;
            this.notifications = notifications;
        }

        public IActionResult Create()
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                notifications.Error(City.OnlyAdminCanCreate);
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var cityFormModel = new CityFormModel
            {
                Countries = countries.All()
            };

            return View(cityFormModel);
        }

        [HttpPost]
        public IActionResult Create(CityFormModel cityModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 || User.IsManager())
            {
                notifications.Error(City.OnlyAdminCanCreate);
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            if (ModelState.IsValid == false)
            {
                cityModel.Countries = countries.All();
                return View(cityModel);
            }

            cityModel.CountryName = ToTitleCase(cityModel.CountryName);
            cityModel.Name = ToTitleCase(cityModel.Name);

            var cityId = cities.Create(cityModel.Name, cityModel.CountryName, adminId);

            if (cityId == 0)
            {
                cityModel.Countries = countries.All();
                notifications.Error(City.ThereAreAlreadyACity);
                return View(cityModel);
            }

            notifications.Success(City.SuccessfullyCreated, DurationInSeconds);
            return RedirectToAction(Game.CreateGameFirstStep, Game.ControllerName);
        }
    }
}
