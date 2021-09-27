namespace MiniFootball.Test.Controllers
{
    using System;
    using System.Globalization;
    using MiniFootball.Controllers;
    using MiniFootball.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class UsersControllerTest
    {
        [Fact]
        public void GetDetailsShouldReturnView()
            => MyController<UsersController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<User>(user => user
                            .Add(new User
                            {
                                Id = TestUser.Identifier,
                                FirstName = "Eray",
                                LastName = "Erol",
                                NickName = "zvpPpPp",
                                Email = "zwp@gmail.com",
                                UserName = "zwp@gmail.com",
                                PhoneNumber = "0886911492",
                                PhotoPath = "https://www.facebook.com/photo?fbid=3996431367092876&set=a.107988282603890",
                                Birthdate = DateTime.ParseExact(
                                    "1995-09-30 14:00:52,531",
                                    "yyyy-MM-dd HH:mm:ss,fff",
                                    CultureInfo.InvariantCulture),
                            }))))
                .Calling(c => c.Details(TestUser.Identifier))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();
    }
}
