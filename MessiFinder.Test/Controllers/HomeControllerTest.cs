namespace MessiFinder.Test.Controllers
{
    using Data.Models;
    using FluentAssertions;
    using MessiFinder.Controllers;
    using MessiFinder.Services.Games;
    using MessiFinder.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using Mocks;
    using Models.Home;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which(controller => controller
                    .WithData(GetGames()))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IndexViewModel>()
                    .Passing(m => m.Games.Should().HaveCount(3)));

        [Fact]
        public void IndexShouldReturnViewWithCorrectModel()
        {
            // Arrange
            var data = DatabaseMock.Instance;
            var mapper = MapperMock.Instance;

            var games = GetGames()
                .ToList();

            data.Games.AddRange(games);
            data.Users.Add(new User());

            data.SaveChanges();

            var gameService = new GameService(data, mapper);
            var statisticsService = new StatisticsService(data);

            var homeController = new HomeController(gameService, statisticsService);

            // Act
            var result = homeController.Index();

            // Assert
            // Assert.NotNull(result);

            // var viewResult = Assert.IsType<ViewResult>(result);

            // var model = viewResult.Model;

            // var indexViewModel = Assert.IsType<IndexViewModel>(model);

            // Assert.Equal(3, indexViewModel.Games.Count);
            // Assert.Equal(10, indexViewModel.TotalGames);
            // Assert.Equal(1, indexViewModel.TotalUsers);

            result
                .Should()
                .NotBeNull()
                .And
                .BeAssignableTo<ViewResult>()
                .Which
                .Model
                .As<IndexViewModel>()
                .Invoking(model =>
                {
                    model.Games.Should().HaveCount(3);
                    model.TotalGames.Should().Be(10);
                    model.TotalUsers.Should().Be(1);
                })
                .Invoke();
        }

        [Fact]
        public void ErrorShouldReturnView()
        {
            // Arrange
            var homeController = new HomeController(null, null);

            // Act
            var result = homeController.Error();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        private static IEnumerable<Game> GetGames()
            => Enumerable
                .Range(0, 10)
                .Select(i => new Game
                {
                    Playground = new Playground()
                });
    }
}
