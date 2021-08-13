namespace MiniFootball.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cities;
    using Services.Admins;
    using Services.Cities;
    using Services.Countries;

    using static MyWebCase;
    using static WebConstants;

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
                TempData[GlobalMessageKey] = "Only Admins can create city!";
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
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
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                cityModel.Countries = countries.All();
                return View(cityModel);
            }

            cityModel.CountryName = FirstLetterUpperThenLower(cityModel.CountryName);
            cityModel.Name = FirstLetterUpperThenLower(cityModel.Name);

            var cityId = cities.Create(cityModel.Name, cityModel.CountryName, adminId);

            if (cityId == 0)
            {
                cityModel.Countries = countries.All();

                TempData[GlobalMessageKey] = "There are already a City with this Name and Country!";
                return View(cityModel);
            }

            TempData[GlobalMessageKey] = "You created city!";
            return RedirectToAction("CreateGameFirstStep", "Games");
        }
    }
}
