//namespace MiniFootball.Test.Controllers
//{
//    using MiniFootball.Controllers;
//    using Models.Games;
//    using MyTested.AspNetCore.Mvc;
//    using Services.Games;
//    using Services.Games.Models;
//    using Xunit;

//    public class GamesControllerTest
//    {
//        [Fact]
//        public void AllShouldReturnView()
//            => MyController<GamesController>
//                .Instance()
//                .Calling(c => c.All(new GameAllQueryModel()))
//                .ShouldReturn()
//                .View();

//        [Fact]
//        public void CreateGameFirstStepShouldBeForAuthorizedUsers()
//            => MyController<GamesController>
//                .Instance()
//                .Calling(c => c.CreateGameFirstStep(new CreateGameFirstStepViewModel()))
//                .ShouldHave()
//                .ActionAttributes(attributes => attributes
//                    .RestrictingForAuthorizedRequests())
//                .AndAlso()
//                .ShouldReturn()
//                .Redirect(redirect => redirect
//                    .To<AdminsController>(c => c.Become()));

//        [Fact]
//        public void CreateGameChooseFieldShouldBeForAuthorizedUsers()
//            => MyController<GamesController>
//                .Instance()
//                .Calling(c => c.CreateGameChooseField(new CreateGameCountryAndCityViewModel()))
//                .ShouldHave()
//                .ActionAttributes(attributes => attributes
//                    .RestrictingForAuthorizedRequests())
//                .AndAlso()
//                .ShouldReturn()
//                .View();

//        //[Fact]
//        //public void CreateGameLastStepShouldBeForAuthorizedUsers()
//        //    => MyController<GamesController>
//        //        .Instance(controller => controller
//        //            .WithData())
//        //        .Calling(c => c.CreateGameLastStep(new CreateGameLastStepViewModel()))
//        //        .ShouldHave()
//        //        .ActionAttributes(attributes => attributes
//        //            .RestrictingForAuthorizedRequests())
//        //        .AndAlso()
//        //        .ShouldReturn()
//        //        .Redirect(redirect => redirect
//        //            .To<AdminsController>(c => c.Become()));

//        [Fact]
//        public void GetEditShouldBeForAuthorizedUsers()
//            => MyController<GamesController>
//                .Instance()
//                .Calling(c => c.Edit(new GameEditServiceModel()))
//                .ShouldHave()
//                .ActionAttributes(attributes => attributes
//                    .RestrictingForAuthorizedRequests())
//                .AndAlso()
//                .ShouldReturn()
//                .Redirect(redirect => redirect
//                    .To<AdminsController>(c => c.Become()));

//        [Fact]
//        public void GetDeleteShouldBeForAuthorizedUsers()
//            => MyController<GamesController>
//                .Instance()
//                .Calling(c => c.Delete(new GameDeleteServiceModel()))
//                .ShouldHave()
//                .ActionAttributes(attributes => attributes
//                    .RestrictingForAuthorizedRequests())
//                .AndAlso()
//                .ShouldReturn()
//                .Redirect(redirect => redirect
//                    .To<AdminsController>(c => c.Become()));

//        //[Fact]
//        //public void GetDetailsShouldBeForAuthorizedUsers()
//        //    => MyController<GamesController>
//        //        .Instance()
//        //        .Calling(c => c.Details(1, "asd"))
//        //        .ShouldHave()
//        //        .ActionAttributes(attributes => attributes
//        //            .RestrictingForAuthorizedRequests())
//        //        .AndAlso()
//        //        .ShouldReturn()
//        //        .Redirect(redirect => redirect
//        //            .To<AdminsController>(c => c.Become()));

//        [Fact]
//        public void GetMineShouldBeForAuthorizedUsers()
//            => MyController<GamesController>
//                .Instance()
//                .Calling(c => c.Mine())
//                .ShouldHave()
//                .ActionAttributes(attributes => attributes
//                    .RestrictingForAuthorizedRequests())
//                .AndAlso()
//                .ShouldReturn()
//                .Redirect(redirect => redirect
//                    .To<AdminsController>(c => c.Become()));

//    }
//}
