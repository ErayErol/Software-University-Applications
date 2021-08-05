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

    public class GamesController : Controller
    {
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
            IMapper mapper)
        {
            this.countries = countries;
            this.games = games;
            this.admins = admins;
            this.fields = fields;
            this.mapper = mapper;
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

            // TODO : Do this for all
            gameForm.Town =
                gameForm.Town[0].ToString().ToUpper()
                + gameForm.Town.Substring(1, gameForm.Town.Length - 1).ToLower();

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

            if (ModelState.IsValid == false)
            {
                return View(gameCreateModel);
            }

            // TODO: There is already exist game in this fields in this date
            // TODO: Add appointment like beautybooking with only times like 20:00, 21:00 and the user can pick up time... do it more beautiful when create game, cause date is a little bit ugly

            this.games.Create(
                gameCreateModel.FieldId,
                gameCreateModel.Description,
                gameCreateModel.Date.Value,
                gameCreateModel.NumberOfPlayers.Value,
                gameCreateModel.Goalkeeper,
                gameCreateModel.Ball,
                gameCreateModel.Jerseys,
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
            var myGames = this.games
                .ByUser(this.User.Id())
                .OrderByDescending(g => g.Date.Date);

            return View(myGames);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            // TODO: This edit only Game, but field is same 
            // When we are in edit page, add button for edit field
            // and then return RedirectToAction(nameof(EditPlayground));

            // TODO: Add Free places too
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

            // TODO: Do this check for Edit Fields also only manager can edit fields
            //if (this.fields.FieldExist(game.FieldId) == false)
            //{
            //    this.ModelState.AddModelError(nameof(game.FieldId), "Field does not exist.");
            //}

            if (ModelState.IsValid == false)
            {
                return View(game);
            }

            if (this.games.IsByAdmin(id, adminId) == false && this.User.IsManager() == false)
            {
                return BadRequest();
            }

            this.games.Edit(
                id,
                game.Date,
                game.NumberOfPlayers,
                game.Ball,
                game.Jerseys,
                game.Goalkeeper,
                game.Description);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            // TODO: Add validation and telephoneNumbers for contact

            var game = this.games.GetDetails(id);

            // TODO: Create GameInfoModel not use GameFormModel for details
            var gameForm = this.mapper.Map<GameFormModel>(game);

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
            // TODO : Add validation and manager have to see all players without join

            var playersName = this.games.SeePlayers(id);

            if (ModelState.IsValid == false)
            {
                return RedirectToAction(nameof(Details));
            }

            return View(playersName);
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            var userId = this.User.Id();

            if (this.admins.IsAdmin(userId) == false && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            // TODO : Create Model and do not use GameDetailsServiceModel, cause you take so much info, you only need Id and UserId
            var game = this.games.GetDetails(id);

            if (game.UserId != userId && this.User.IsManager() == false)
            {
                return Unauthorized();
            }

            return View(game);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(GameDetailsServiceModel gameDetails)
        {
            // TODO: I code it fast! Look at, maybe you can refactor it, but maybe Im not sure....
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
