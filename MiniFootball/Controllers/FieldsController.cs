namespace MiniFootball.Controllers
{
    using AutoMapper;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Fields;
    using Services.Cities;
    using Services.Countries;
    using Services.Fields;

    using static MyWebCase;
    using static WebConstants;

    public class FieldsController : Controller
    {
        private readonly ICountryService countries;
        private readonly ICityService cities;
        private readonly IFieldService fields;
        private readonly IMapper mapper;

        public FieldsController(
            ICountryService countries,
            IFieldService fields, 
            ICityService cities, 
            IMapper mapper)
        {
            this.countries = countries;
            this.fields = fields;
            this.cities = cities;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Create()
        {
            if (this.User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = "Only Manager can create fields!";
                return View();
            }

            return View(new FieldCreateFormModel
            {
                Countries = this.countries.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(FieldCreateFormModel fieldModel)
        {
            if (this.User.IsManager() == false)
            {
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
                fieldModel.Description);

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
    }
}
