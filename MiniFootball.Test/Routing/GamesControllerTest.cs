namespace MiniFootball.Test.Routing
{
    using MiniFootball.Controllers;
    using Models.Games;
    using MyTested.AspNetCore.Mvc;
    using Services.Games.Models;
    using Xunit;

    public class GamesControllerTest
    {
        [Fact]
        public void AllRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/All"))
                .To<GamesController>(c => c.All(With.Any<GameAllQueryModel>()));

        [Fact]
        public void GetCreateGameFirstStepRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/CreateGameFirstStep"))
                .To<GamesController>(c => c.CreateGameFirstStep());

        [Fact]
        public void PostCreateGameFirstStepRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/CreateGameFirstStep")
                    .WithMethod(HttpMethod.Post))
                .To<GamesController>(c => c.CreateGameFirstStep(With.Any<CreateGameFirstStepViewModel>()));

        [Fact]
        public void GetCreateGameChooseFieldRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/CreateGameChooseField"))
                .To<GamesController>(c => c.CreateGameChooseField(With.Any<CreateGameCountryAndCityViewModel>()));

        [Fact]
        public void PostCreateGameChooseFieldRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/CreateGameChooseField")
                    .WithMethod(HttpMethod.Post))
                .To<GamesController>(c => c.CreateGameChooseField(With.Any<CreateGameSecondStepViewModel>()));

        [Fact]
        public void GetCreateGameLastStepRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/CreateGameLastStep"))
                .To<GamesController>(c => c.CreateGameLastStep(With.Any<CreateGameFormModel>()));

        [Fact]
        public void GetEditRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/Edit"))
                .To<GamesController>(c => c.Edit(With.Any<GameEditServiceModel>()));

        [Fact]
        public void PostEditRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/Edit")
                    .WithMethod(HttpMethod.Post))
                .To<GamesController>(c => c.Edit(With.Any<GameEditServiceModel>()));

        [Fact]
        public void GetDeleteRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/Delete"))
                .To<GamesController>(c => c.Delete(With.Any<GameDeleteServiceModel>()));

        [Fact]
        public void PostDeleteRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/Delete")
                    .WithMethod(HttpMethod.Post))
                .To<GamesController>(c => c.Delete(With.Any<GameDeleteServiceModel>()));

        [Theory]
        [InlineData("GameId123", "Avenue")]
        public void DetailsRouteShouldBeMapped(string id, string info)
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath($"/Games/Details/{id}/{info}"))
                .To<GamesController>(c => c.Details(id, info));

        [Fact]
        public void MineRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Games/Mine"))
                .To<GamesController>(c => c.Mine());
    }
}
