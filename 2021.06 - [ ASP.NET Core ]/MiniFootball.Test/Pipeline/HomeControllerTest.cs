namespace MiniFootball.Test.Pipeline
{
    using FluentAssertions;
    using MiniFootball.Controllers;
    using MiniFootball.Services.Games.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Xunit;

    using static Data.Games;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which(controller => controller
                    .WithData(TenPublicGames()))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<GameListingServiceModel>>()
                    .Passing(m => m.Should().HaveCount(3)));
    }
}