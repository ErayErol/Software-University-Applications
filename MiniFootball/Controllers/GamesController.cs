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
    using Areas.Admin.Controllers;
    using Services.Cities;
    using static Convert;
    using static WebConstants;
    using static GlobalConstant;

    public class GamesController : Controller
    {
        private readonly IUserService users;
        private readonly ICountryService countries;
        private readonly IGameService games;
        private readonly IAdminService admins;
        private readonly ICityService cities;
        private readonly IFieldService fields;
        private readonly IMapper mapper;

        public GamesController(
            ICountryService countries,
            IGameService games,
            IAdminService admins,
            IFieldService fields,
            IMapper mapper,
            IUserService users,
            ICityService cities)
        {
            this.countries = countries;
            this.games = games;
            this.admins = admins;
            this.fields = fields;
            this.mapper = mapper;
            this.users = users;
            this.cities = cities;
        }

        public IActionResult All([FromQuery] GameAllQueryModel query)
        {
            var queryResult = games
                .All(
                    query.City,
                    query.SearchTerm,
                    query.Sorting,
                    query.CurrentPage,
                    query.GamesPerPage);

            var cities = fields.AllCreatedCitiesName();

            query.TotalGames = queryResult.TotalGames;
            query.Games = queryResult.Games;
            query.Cities = cities;

            return View(query);
        }

        [Authorize]
        public IActionResult CreateGameFirstStep()
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                TempData[GlobalMessageKey] = Game.OnlyAdminCanCreate;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            return View(new CreateGameFirstStepViewModel
            {
                Countries = countries.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateGameFirstStep(CreateGameFirstStepViewModel gameModel)
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                TempData[GlobalMessageKey] = Game.OnlyAdminCanCreate;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            if (ModelState.IsValid == false)
            {
                gameModel.Countries = countries.All();
                return View(gameModel);
            }

            gameModel.CityName = ToTitleCase(gameModel.CityName);

            var cityId = cities.CityIdByName(gameModel.CityName);

            if (cityId == 0)
            {
                TempData[GlobalMessageKey] = City.CityDoesNotExistInCountry;
                return RedirectToAction(Create, City.ControllerName);
            }

            return RedirectToAction(
                Game.CreateGameChooseField,
                Game.ControllerName,
                new CreateGameCountryAndCityViewModel
                {
                    CityName = gameModel.CityName,
                    CountryName = gameModel.CountryName,
                });
        }

        [Authorize]
        public IActionResult CreateGameChooseField(CreateGameCountryAndCityViewModel gameModel)
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                return View();
            }

            return View(new CreateGameSecondStepViewModel
            {
                Fields = fields.FieldsListing(gameModel.CityName, gameModel.CountryName),
                CityName = gameModel.CityName,
                CountryName = gameModel.CountryName,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateGameChooseField(CreateGameSecondStepViewModel gameModel)
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                TempData[GlobalMessageKey] = Game.OnlyAdminCanCreate;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var fieldId = gameModel.FieldId;

            if (fields.FieldExist(fieldId) == false)
            {
                TempData[GlobalMessageKey] = Field.DoesNotExist;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            var fieldName = fields.FieldName(fieldId);
            gameModel.Name = fieldName;

            var lastStepGame = mapper.Map<CreateGameLastStepViewModel>(gameModel);

            return RedirectToAction(
                Game.CreateGameLastStep, 
                Game.ControllerName, 
                lastStepGame);
        }

        [Authorize]
        public IActionResult CreateGameLastStep(CreateGameLastStepViewModel gameModel)
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                TempData[GlobalMessageKey] = Game.OnlyAdminCanCreate;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            if (fields.IsCorrectParameters(
                gameModel.FieldId,
                gameModel.Name,
                gameModel.CountryName,
                gameModel.CityName) == false)
            {
                TempData[GlobalMessageKey] = Field.IncorrectParameters;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            return View(new CreateGameFormModel
            {
                FieldId = gameModel.FieldId,
                FieldName = fields.FieldName(gameModel.FieldId)
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateGameLastStep(CreateGameFormModel gameModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 || User.IsManager())
            {
                TempData[GlobalMessageKey] = Game.OnlyAdminCanCreate;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            if (ModelState.IsValid == false)
            {
                return View(gameModel);
            }

            var correctDate = gameModel.Date != null && gameModel.Date.Value < DateTime.Today;

            if (correctDate)
            {
                TempData[GlobalMessageKey] = Game.IncorrectDate;
                return View(gameModel);
            }

            var reserved = games
                .IsFieldAlreadyReserved(gameModel.FieldId, gameModel.Date.Value, gameModel.Time.Value);

            if (reserved)
            {
                TempData[GlobalMessageKey] = Game.ThereAreAlreadyAGame;
                return View(gameModel);
            }


            gameModel.Description = ToSentenceCase(gameModel.Description);
            var phoneNumber = users.UserDetails(User.Id()).PhoneNumber;

            var gameId = games.Create(
                gameModel.FieldId,
                gameModel.Date.Value,
                gameModel.Time.Value,
                gameModel.NumberOfPlayers.Value,
                gameModel.FacebookUrl,
                gameModel.Ball,
                gameModel.Jerseys,
                gameModel.Goalkeeper,
                gameModel.Description,
                gameModel.Places,
                gameModel.HasPlaces,
                adminId,
                phoneNumber);

            if (gameId == string.Empty)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            TempData[GlobalMessageKey] = Game.SuccessfullyCreated;
            return RedirectToAction(Game.Mine);
        }

        [Authorize]
        public IActionResult Edit(string gameId, string information)
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Game.OnlyCreatorCanEdit;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var gameDetails = games.GetDetails(gameId);

            if (gameDetails.FieldName.Equals(information) == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            if (gameDetails?.UserId != userId && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = "Only the creator of the game or moderator can edit the game!";
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            var gameEdit = mapper.Map<GameEditServiceModel>(gameDetails);

            return View(gameEdit);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(GameEditServiceModel gameModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Game.OnlyCreatorCanEdit;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            if (games.IsAdminCreatorOfGame(gameModel.GameId, adminId) == false && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Game.OnlyCreatorCanEdit;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            if (ModelState.IsValid == false)
            {
                return View(gameModel);
            }

            var isEdit = games.Edit(
                gameModel.GameId,
                gameModel.Date,
                gameModel.Time,
                gameModel.NumberOfPlayers,
                gameModel.FacebookUrl,
                gameModel.Ball,
                gameModel.Jerseys,
                gameModel.Goalkeeper,
                gameModel.Description,
                User.IsManager());

            if (isEdit == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            TempData[GlobalMessageKey] = 
                $"Your game was edited{(User.IsManager() ? string.Empty : " and is awaiting approval")}!";

            return RedirectToAction(nameof(Details),
                new { gameId = gameModel.GameId, information = gameModel.FieldName });
        }

        [Authorize]
        public IActionResult Delete(string gameId, string information)
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Game.OnlyCreatorCanDelete;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var adminId = admins.IdByUser(User.Id());

            var isAdminCreatorOfGame = games.IsAdminCreatorOfGame(gameId, adminId);

            if (isAdminCreatorOfGame == false && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Game.OnlyCreatorCanDelete;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            var gameDeleteDetails = games.GameDeleteInfo(gameId);

            if (gameDeleteDetails.FieldName.Equals(information) == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            return View(gameDeleteDetails);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(GameDeleteServiceModel gameModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Game.OnlyCreatorCanDelete;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var isAdminCreatorOfGame = games.IsAdminCreatorOfGame(gameModel.GameId, adminId);

            if (isAdminCreatorOfGame == false && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Game.OnlyCreatorCanDelete;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            if (games.Delete(gameModel.GameId) == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            TempData[GlobalMessageKey] = Game.SuccessfullyDeleted;
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult AddUserToGame(string gameId)
        {
            if (ModelState.IsValid == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(nameof(Details));
            }

            if (games.AddUserToGame(gameId, User.Id()) == false)
            {
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            TempData[GlobalMessageKey] = Game.SuccessfullyJoined;
            return Redirect($"SeePlayers?gameId={gameId}");
        }

        [Authorize]
        public IActionResult SeePlayers(string gameId)
        {
            var userId = User.Id();

            if (games.IsUserIsJoinGame(gameId, userId) == false && User.IsManager() == false)
            {
                var gameIdUserId = games.GameDeleteInfo(gameId);

                if (gameIdUserId.UserId != userId)
                {
                    TempData[GlobalMessageKey] = Game.OnlyJoinedPlayersCanView;
                    return RedirectToAction(nameof(All));
                }
            }

            var joinedPlayers = games.SeePlayers(gameId);

            if (joinedPlayers.Any() == false)
            {
                TempData[GlobalMessageKey] = Game.YetNoPlayers;
            }

            return View(joinedPlayers);
        }

        [Authorize]
        public IActionResult ExitGame(string gameId, string userIdToDelete)
        {
            var currentUserId = User.Id();
            var currentUserAdminId = admins.IdByUser(currentUserId);

            if (admins.IsAdmin(currentUserId) == false
                && User.IsManager() == false
                && currentUserId != userIdToDelete)
            {
                TempData[GlobalMessageKey] = Game.YouCanNotRemovePlayer;
                return RedirectToAction(nameof(All));
            }

            if (currentUserAdminId == 0
                && User.IsManager() == false
                && currentUserId != userIdToDelete)
            {
                TempData[GlobalMessageKey] = Game.YouCanNotRemovePlayer;
                return RedirectToAction(nameof(All));
            }

            if (games.IsAdminCreatorOfGame(gameId, currentUserAdminId) == false
                && User.IsManager() == false
                && currentUserId != userIdToDelete)
            {
                TempData[GlobalMessageKey] = Game.YouCanNotRemovePlayer;
                return RedirectToAction(nameof(All));
            }

            var isUserRemoved = games.RemoveUserFromGame(gameId, userIdToDelete);

            if (isUserRemoved == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            TempData[GlobalMessageKey] = Game.UserExit;
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Details(string gameId, string information)
        {
            var gameDetails = games.GetDetails(gameId);

            if (gameDetails == null || gameDetails.FieldName.Equals(information) == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            if (games.IsUserIsJoinGame(gameId, User.Id()))
            {
                gameDetails.IsUserAlreadyJoin = true;
            }

            return View(gameDetails);
        }

        [Authorize]
        public IActionResult Mine()
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var myGames = games
                .GamesWhereCreatorIsUser(User.Id())
                .OrderByDescending(g => g.Date.Date);

            if (myGames.Any() == false)
            {
                TempData[GlobalMessageKey] = Game.YouDoNotHaveAnyGamesYet;
            }

            return View(myGames);
        }
    }
}
