namespace MiniFootball.Test.Routing
{
    using Areas.Admin.Controllers;
    using Models.Admins;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class AdminsControllerTest
    {
        [Fact]
        public void GetBecomeRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Admins/Become"))
                .To<AdminsController>(c => c.Become());

        [Fact]
        public void PostBecomeRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Admins/Become")
                    .WithMethod(HttpMethod.Post))
                .To<AdminsController>(c => c.Become(With.Any<BecomeAdminFormModel>()));
    }
}
