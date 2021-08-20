namespace MiniFootball.Test.Controllers
{
    using FluentAssertions;
    using MiniFootball.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Services.Games.Models;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using static Data.Games;
    using static WebConstants.Cache;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModel()
            => MyController<HomeController>
                .Instance(controller => controller
                    .WithData(TenPublicGames()))
                .Calling(c => c.Index())
                .ShouldHave()
                .MemoryCache(cache => cache
                    .ContainingEntry(entry => entry
                        .WithKey(LatestGamesCacheKey)
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(15))
                        .WithValueOfType<List<GameListingServiceModel>>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<GameListingServiceModel>>()
                    .Passing(m => m.Should().HaveCount(3)));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Error())
                .ShouldReturn()
                .View();
    }
}
