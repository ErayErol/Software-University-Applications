namespace MiniFootball.Controllers
{
    using Areas.Admin.Controllers;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cities;
    using Services.Admins;
    using Services.Cities;
    using Services.Countries;

    using static Convert;
    using static WebConstants;
    using static GlobalConstant;

    public class CitiesController : Controller
    {
        private readonly ICountryService countries;
        private readonly ICityService cities;
        private readonly IAdminService admins;

        public CitiesController(
            ICountryService countries, 
            ICityService cities, 
            IAdminService admins)
        {
            this.countries = countries;
            this.cities = cities;
            this.admins = admins;
        }

        [Authorize]
        public IActionResult Create()
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                TempData[GlobalMessageKey] = City.OnlyAdminCanCreate;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            return View(new CityFormModel
            {
                Countries = countries.All()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CityFormModel cityModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 || User.IsManager())
            {
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

                TempData[GlobalMessageKey] = City.ThereAreAlreadyACity;
                return View(cityModel);
            }

            TempData[GlobalMessageKey] = City.SuccessfullyCreated;
            return RedirectToAction(Game.CreateGameFirstStep, Game.ControllerName);
        }
    }
}
