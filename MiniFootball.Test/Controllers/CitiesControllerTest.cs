namespace MiniFootball.Test.Controllers
{
    using MiniFootball.Controllers;
    using Models.Cities;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class CitiesControllerTest
    {
        [Fact]
        public void GetCreateShouldBeForAuthorizedUsers()
            => MyController<CitiesController>
                .Instance()
                .Calling(c => c.Create(new CityFormModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));
    }
}
