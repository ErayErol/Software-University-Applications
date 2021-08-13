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
        public IActionResult CountryListing(CreateGameFirstStepViewModel gameModel)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                TempData[GlobalMessageKey] = "Only Admins can create fields!";
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                gameModel.Countries = this.countries.All();
                return View(gameModel);
            }

            gameModel.CityName = FirstLetterUpperThenLower(gameModel.CityName);

            var isExist = this.countries
                .Cities(gameModel.CountryName)?
                .Any(c => c == gameModel.CityName);

            if (isExist == false)
            {
                TempData[GlobalMessageKey] =
                    "This City does not exist in this Country. First you have to create City and then create Game!";
                return RedirectToAction("Create", "Cities");
            }

            return RedirectToAction("FieldListing", "Games", new CreateGameFirstStepViewModel
            {
                CityName = gameModel.CityName,
                CountryName = gameModel.CountryName,
            });
        }

        [Authorize]
        public IActionResult FieldListing(CreateGameFirstStepViewModel gameModel)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                return View();
            }

            return View(new FieldListingViewModel
            {
                Fields = this.fields.FieldsListing(gameModel.CityName, gameModel.CountryName),
                CityName = gameModel.CityName,
                CountryName = gameModel.CountryName,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult FieldListing(FieldListingViewModel gameModel)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                return BadRequest();
            }

            var fieldId = gameModel.FieldId;

            if (this.fields.FieldExist(fieldId) == false)
            {
                return BadRequest();
            }

            var fieldName = this.fields.FieldName(fieldId);
            gameModel.Name = fieldName;

            return RedirectToAction("Create", "Games", gameModel);
        }

        [Authorize]
        public IActionResult Create(FieldListingViewModel gameModel)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false || this.User.IsManager())
            {
                return View();
            }

            if (this.fields.IsCorrectCountryAndCity(
                gameModel.FieldId,
                gameModel.Name,
                gameModel.CountryName,
                gameModel.CityName) == false)
            {
                return BadRequest();
            }

            return View(new GameFormModel
            {
                FieldId = gameModel.FieldId,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(GameFormModel gameModel)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 || this.User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                return View(gameModel);
            }

            if (gameModel.Date != null && gameModel.Date.Value < DateTime.Today)
            {
                TempData[GlobalMessageKey] = "The date has to be today or after today!";
                return View(gameModel);
            }

            var game = this.games.IsExist(gameModel.FieldId, gameModel.Date.Value, gameModel.Time.Value);

            if (game)
            {
                TempData[GlobalMessageKey] = "There are already a game in this field in this date and time!";
                return View(gameModel);
            }

            var id = this.games.Create(
                gameModel.FieldId,
                gameModel.Date.Value,
                gameModel.Time.Value,
                gameModel.NumberOfPlayers.Value,
                gameModel.FacebookUrl,
                gameModel.Ball,
                gameModel.Jerseys,
                gameModel.Goalkeeper,
                gameModel.Description,
                gameModel.NumberOfPlayers.Value,
                true,
                adminId);

            if (id == null)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You created game!";
            return Redirect($"Details?id={id}");
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

            if (game?.UserId != userId && this.User.IsManager() == false)
            {
                return Unauthorized();
            }

            var gameForm = this.mapper.Map<GameFormModel>(game);

            return View(gameForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(GameFormModel gameModel)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 || this.User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                return View(gameModel);
            }

            if (this.games.IsByAdmin(gameModel.Id, adminId) == false && this.User.IsManager() == false)
            {
                return BadRequest();
            }

            var isEdit = this.games.Edit(
                gameModel.Id,
                gameModel.Date,
                gameModel.Time,
                gameModel.NumberOfPlayers,
                gameModel.FacebookUrl,
                gameModel.Ball,
                gameModel.Jerseys,
                gameModel.Goalkeeper,
                gameModel.Description);

            if (isEdit == false)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You edited game!";
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
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
        public IActionResult Delete(GameIdUserIdServiceModel gameModel)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (this.games.IsByAdmin(gameModel.Id, adminId) == false && this.User.IsManager() == false)
            {
                return BadRequest();
            }

            if (this.games.Delete(gameModel.Id) == false)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "You deleted game!";
            return RedirectToAction(nameof(All));
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

            if (players.Any() == false)
            {
                TempData[GlobalMessageKey] = "Still there are no players for this game!";
                return Redirect($"Details?id={id}");
            }

            return View(players);
        }

        [Authorize]
        public IActionResult ExitGame([FromQuery]string gameIdUserId)
        {
            var currentUserId = this.User.Id();
            var splitQuery = gameIdUserId.Split('*');
            var gameId = splitQuery[0];
            var userIdToDelete = splitQuery[1];
            var currentUserAdminId = this.admins.IdByUser(currentUserId);

            if (this.admins.IsAdmin(currentUserId) == false
                && this.User.IsManager() == false
                && currentUserId != userIdToDelete)
            {
                TempData[GlobalMessageKey] = "You can not remove player from this game!";
                return RedirectToAction(nameof(All));
            }

            if (currentUserAdminId == 0
                && this.User.IsManager() == false
                && currentUserId != userIdToDelete)
            {
                TempData[GlobalMessageKey] = "You can not remove player from this game!";
                return RedirectToAction(nameof(All));
            }

            if (this.games.IsByAdmin(gameId, currentUserAdminId) == false
                && this.User.IsManager() == false
                && currentUserId != userIdToDelete)
            {
                TempData[GlobalMessageKey] = "You can not remove player from this game!";
                return RedirectToAction(nameof(All));
            }

            var isUserRemoved = this.games.RemoveUserFromGame(gameId, userIdToDelete);

            if (isUserRemoved == false)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "User exit from game!";
            return RedirectToAction("All");
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            // TODO: REFACTOR THIS
            var game = this.games.GetDetails(id);

            var gameForm = this.mapper.Map<GameFormModel>(game);

            // TODO: THIS PHONE NUMBER IS INCORRECT
            gameForm.PhoneNumber = this.users.UserDetails(this.User.Id()).PhoneNumber;

            if (this.games.IsUserIsJoinGame(id, this.User.Id()))
            {
                gameForm.IsUserAlreadyJoin = true;
            }

            return View(gameForm);
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

            if (myGames.Any() == false)
            {
                TempData[GlobalMessageKey] = "Still you do not have games, but you can create it!";
            }

            return View(myGames);
        }
    }
}
