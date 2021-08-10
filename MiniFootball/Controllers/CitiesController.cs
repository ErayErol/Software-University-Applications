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
            if (this.admins.IsAdmin(this.User.Id()) == false && this.User.IsManager() == false)
            {
                return BadRequest();
            }

            return View(new CityFormModel
            {
                Countries = this.countries.All()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CityFormModel cityFormModel)
        {
            if (ModelState.IsValid == false)
            {
                cityFormModel.Countries = this.countries.All();
                return View(cityFormModel);
            }

            cityFormModel.CountryName = FirstLetterUpperThenLower(cityFormModel.CountryName);
            cityFormModel.Name = FirstLetterUpperThenLower(cityFormModel.Name);

            var cityId = this.cities.Create(cityFormModel.Name, cityFormModel.CountryName);

            if (cityId == 0)
            {
                cityFormModel.Countries = this.countries.All();

                TempData[GlobalMessageKey] = "There are already a City with this Name and Country!";
                return View(cityFormModel);
            }

            TempData[GlobalMessageKey] = "You created city!";
            return RedirectToAction("CountryListing", "Games");
        }
    }
}
