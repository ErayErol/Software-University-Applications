namespace MiniFootball.Controllers
{
    using System;
    using AutoMapper;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Games;
    using Services.Admins;
    using Services.Countries;
    using Services.Fields;
    using Services.Games;
    using Services.Games.Models;
    using Services.Users;
    using System.Linq;
    using static MyWebCase;
    using static WebConstants;

    public class GamesController : Controller
    {
        private readonly IUserService users;
        private readonly ICountryService countries;
        private readonly IGameService games;
        private readonly IAdminService admins;
        private readonly IFieldService fields;
        private readonly IMapper mapper;

        public GamesController(
            ICountryService countries,
            IGameService games,
            IAdminService admins,
            IFieldService fields,
            IMapper mapper,
            IUserService users)
        {
            this.countries = countries;
            this.games = games;
            this.admins = admins;
            this.fields = fields;
            this.mapper = mapper;
            this.users = users;
        }

        [Authorize]
        public IActionResult CountryListing()
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                TempData[GlobalMessageKey] = "Only Admins can create games!";
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            return View(new CreateGameFirstStepViewModel
            {
                Countries = this.countries.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult CountryListing(CreateGameFirstStepViewModel gameForm)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                TempData[GlobalMessageKey] = "Only Admins can create fields!";
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                gameForm.Countries = this.countries.All();
                return View(gameForm);
            }

            gameForm.CityName = FirstLetterUpperThenLower(gameForm.CityName);

            var isExist = this.countries
                .Cities(gameForm.CountryName)?
                .Any(c => c == gameForm.CityName);

            if (isExist == false)
            {
                TempData[GlobalMessageKey] =
                    "This City does not exist in this Country. First you have to create City and then create Game!";
                return RedirectToAction("Create", "Cities");
            }

            // TODO: Maybe you have to create view model that pass only cityName and countryName
            return RedirectToAction("FieldListing", "Games", new CreateGameFirstStepViewModel
            {
                CityName = gameForm.CityName,
                CountryName = gameForm.CountryName,
            });
        }

        [Authorize]
        public IActionResult FieldListing(CreateGameFirstStepViewModel gameForm)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                return View();
            }

            return View(new FieldListingViewModel
            {
                Fields = this.fields.FieldsListing(gameForm.CityName, gameForm.CountryName),
                CityName = gameForm.CityName,
                CountryName = gameForm.CountryName,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult FieldListing(FieldListingViewModel gamePlaygroundModel)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                return BadRequest();
            }

            var fieldId = gamePlaygroundModel.FieldId;

            if (this.fields.FieldExist(fieldId) == false)
            {
                return BadRequest();
            }

            var fieldName = this.fields.FieldName(fieldId);
            gamePlaygroundModel.Name = fieldName;

            return RedirectToAction("Create", "Games", gamePlaygroundModel);
        }

        [Authorize]
        public IActionResult Create(FieldListingViewModel gameForm)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                return View();
            }

            if (this.fields.IsCorrectCountryAndCity(
                gameForm.FieldId,
                gameForm.Name,
                gameForm.CountryName,
                gameForm.CityName) == false)
            {
                return BadRequest();
            }

            return View(new GameFormModel
            {
                FieldId = gameForm.FieldId,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(GameFormModel gameCreateModel)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 || this.User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            // TODO: When facebook url or image url is invalid, the messages is ugly

            if (ModelState.IsValid == false)
            {
                return View(gameCreateModel);
            }

            if (gameCreateModel.Date != null && gameCreateModel.Date.Value < DateTime.Today)
            {
                TempData[GlobalMessageKey] = "The date has to be today or after today!";
                return View(gameCreateModel);
            }

            bool isExist = this.games
                .IsExist(gameCreateModel.FieldId, gameCreateModel.Date.Value, gameCreateModel.Time.Value);

            if (isExist)
            {
                TempData[GlobalMessageKey] = "There are already a game in this field in this date and time!";
                return View(gameCreateModel);
            }

            var id = this.games.Create(
                gameCreateModel.FieldId,
                gameCreateModel.Date.Value,
                gameCreateModel.Time.Value,
                gameCreateModel.NumberOfPlayers.Value,
                gameCreateModel.FacebookUrl,
                gameCreateModel.Ball,
                gameCreateModel.Jerseys,
                gameCreateModel.Goalkeeper,
                gameCreateModel.Description,
                gameCreateModel.NumberOfPlayers.Value,
                true,
                adminId);

            TempData[GlobalMessageKey] = "You created game!";
            return Redirect($"Details?id={id}");
        }

        public IActionResult All([FromQuery] GameAllQueryModel query)
        {
            var queryResult = this.games
                .All(
                    query.City,
                    query.SearchTerm,
                    query.Sorting,
                    query.CurrentPage,
                    query.GamesPerPage);

            var cities = this.fields.Cities();

            query.TotalGames = queryResult.TotalGames;
            query.Games = queryResult.Games;
            query.Cities = cities;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var myGames = this.games
                .ByUser(this.User.Id())
                .OrderByDescending(g => g.Date.Date);

            return View(myGames);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var userId = this.User.Id();

            if (this.admins.IsAdmin(userId) == false && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var game = this.games.GetDetails(id);

            if (game?.UserId != userId || this.User.IsManager())
            {
                return Unauthorized();
            }

            var gameForm = this.mapper.Map<GameFormModel>(game);

            return View(gameForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(GameFormModel game)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 || this.User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                return View(game);
            }

            if (this.games.IsByAdmin(game.Id, adminId) == false && this.User.IsManager() == false)
            {
                return BadRequest();
            }

            var isEdit = this.games.Edit(
                game.Id,
                game.Date,
                game.Time,
                game.NumberOfPlayers,
                game.FacebookUrl,
                game.Ball,
                game.Jerseys,
                game.Goalkeeper,
                game.Description);

            if (isEdit == false)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You edited game!";
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var game = this.games.GetDetails(id);

            var gameForm = this.mapper.Map<GameFormModel>(game);

            gameForm.PhoneNumber = this.users.UserInfo(this.User.Id()).PhoneNumber;

            if (this.games.IsUserIsJoinGame(id, this.User.Id()))
            {
                gameForm.IsUserAlreadyJoin = true;
            }

            return View(gameForm);
        }

        [Authorize]
        public IActionResult AddUserToGame(string id)
        {
            if (ModelState.IsValid == false)
            {
                return RedirectToAction(nameof(Details));
            }

            if (this.games.AddUserToGame(id, this.User.Id()) == false)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You joined game!";
            return Redirect($"SeePlayers?id={id}");
        }

        [Authorize]
        public IActionResult SeePlayers(string id)
        {
            var userId = this.User.Id();

            var game = this.games.GameIdUserId(id);

            if (this.games.IsUserIsJoinGame(id, userId) == false && this.User.IsManager() == false)
            {
                if (game.UserId != userId)
                {
                    TempData[GlobalMessageKey] = "Only joined players can view the list of players!";
                    return RedirectToAction(nameof(All));
                }
            }

            var players = this.games.SeePlayers(id);

            return View(players);
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            // TODO: Change some button color and size
            var userId = this.User.Id();

            if (this.admins.IsAdmin(userId) == false && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var game = this.games.GameIdUserId(id);

            if (game.UserId != userId && this.User.IsManager() == false)
            {
                return Unauthorized();
            }

            return View(game);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(GameIdUserIdServiceModel gameDetails)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (this.games.IsByAdmin(gameDetails.Id, adminId) == false && this.User.IsManager() == false)
            {
                return BadRequest();
            }

            if (this.games.Delete(gameDetails.Id) == false)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You deleted game!";
            return RedirectToAction(nameof(All));
        }
    }
}
