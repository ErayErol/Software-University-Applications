namespace MiniFootball.Controllers
{
    using System.Linq;
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
            if (this.admins.IsAdmin(this.User.Id()) == false)
            {
                return View();
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
            if (ModelState.IsValid == false)
            {
                gameForm.Countries = this.countries.All();
                return View(gameForm);
            }

            return RedirectToAction("FieldListing", "Games", gameForm);
        }

        [Authorize]
        public IActionResult FieldListing(CreateGameFirstStepViewModel gameForm)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false)
            {
                return View();
            }

            gameForm.Town = FirstLetterUpperThenLower(gameForm.Town);

            return View(new FieldListingViewModel
            {
                Fields = this.fields.FieldsListing(gameForm.Town, gameForm.Country),
                Town = gameForm.Town,
                Country = gameForm.Country,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult FieldListing(FieldListingViewModel gamePlaygroundModel)
        {
            var fieldId = gamePlaygroundModel.FieldId;

            if (this.fields.FieldExist(fieldId) == false)
            {
                return BadRequest();
            }

            if (this.fields.FieldExist(fieldId) == false)
            {
                this.ModelState.AddModelError(nameof(fieldId), "Field does not exist!");
            }

            var fieldName = this.fields.FieldName(fieldId);
            gamePlaygroundModel.Name = fieldName;

            return RedirectToAction("Create", "Games", gamePlaygroundModel);
        }

        [Authorize]
        public IActionResult Create(FieldListingViewModel gameForm)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false)
            {
                return View();
            }

            // TODO: Do this to game too
            if (this.fields.IsCorrectCountryAndTown(
                gameForm.FieldId,
                gameForm.Name,
                gameForm.Country,
                gameForm.Town) == false)
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

            if (adminId == 0 && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            // TODO: When facebook url or image url is invalid, the messages is ugly

            if (ModelState.IsValid == false)
            {
                return View(gameCreateModel);
            }

            // TODO: There is already exist game in this fields in this date
            // TODO: Add appointment like beautybooking with only times like 20:00, 21:00 and the user can pick up time... do it more beautiful when create game, cause date is a little bit ugly

            this.games.Create(
                gameCreateModel.FieldId,
                gameCreateModel.Date.Value,
                gameCreateModel.NumberOfPlayers.Value,
                gameCreateModel.FacebookUrl,
                gameCreateModel.Ball,
                gameCreateModel.Jerseys,
                gameCreateModel.Goalkeeper,
                gameCreateModel.Description,
                gameCreateModel.NumberOfPlayers.Value,
                true,
                adminId);

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] GameAllQueryModel query)
        {
            var queryResult = this.games
                .All(
                    query.Town,
                    query.SearchTerm,
                    query.Sorting,
                    query.CurrentPage,
                    query.GamesPerPage);

            var towns = this.fields.Towns();

            query.TotalGames = queryResult.TotalGames;
            query.Games = queryResult.Games;
            query.Towns = towns;

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

            if (game.UserId != userId && this.User.IsManager() == false)
            {
                return Unauthorized();
            }

            var gameForm = this.mapper.Map<GameFormModel>(game);

            return View(gameForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(string id, GameFormModel game)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            // TODO: Delete this and do it in edit fields
            //if (this.fields.FieldExist(game.Id) == false)
            //{
            //    this.ModelState.AddModelError(nameof(game.Id), "Field does not exist.");
            //}

            if (ModelState.IsValid == false)
            {
                return View(game);
            }

            if (this.games.IsByAdmin(id, adminId) == false && this.User.IsManager() == false)
            {
                return BadRequest();
            }

            var isEdit = this.games.Edit(
                id,
                game.Date,
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

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var game = this.games.GetDetails(id);

            var gameForm = this.mapper.Map<GameFormModel>(game);

            gameForm.PhoneNumber = this.users.UserInfo(this.User.Id()).PhoneNumber;

            // TODO: Admin and moderator can see all users

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

            return RedirectToAction(nameof(All));
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
                    TempData[GlobalMessageKey] = "Only joined players can view the list of players";
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

            return RedirectToAction(nameof(All));
        }
    }
}
