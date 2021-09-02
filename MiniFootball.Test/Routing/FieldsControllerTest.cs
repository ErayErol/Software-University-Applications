namespace MiniFootball.Test.Routing
{
    using MiniFootball.Controllers;
    using Models.Fields;
    using MyTested.AspNetCore.Mvc;
    using Services.Fields;
    using Xunit;

    public class FieldsControllerTest
    {
        [Fact]
        public void AllRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Fields/All"))
                .To<FieldsController>(c => c.All(With.Any<FieldAllQueryModel>()));

        [Fact]
        public void GetCreateRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Fields/Create"))
                .To<FieldsController>(c => c.Create());

        [Fact]
        public void PostCreateRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Fields/Create")
                    .WithMethod(HttpMethod.Post))
                .To<FieldsController>(c => c.Create(With.Any<FieldCreateFormModel>()));

        [Fact]
        public void GetEditRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Fields/Edit"))
                .To<FieldsController>(c => c.Edit(With.Any<FieldDetailServiceModel>()));

        [Fact]
        public void PostEditRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Fields/Edit")
                    .WithMethod(HttpMethod.Post))
                .To<FieldsController>(c => c.Edit(With.Any<FieldDetailServiceModel>()));

        [Fact]
        public void GetDeleteRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Fields/Delete"))
                .To<FieldsController>(c => c.Delete(With.Any<FieldDeleteServiceModel>()));

        [Fact]
        public void PostDeleteRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Fields/Delete")
                    .WithMethod(HttpMethod.Post))
                .To<FieldsController>(c => c.Delete(With.Any<FieldDeleteServiceModel>()));

        [Theory]
        [InlineData(4, "Avenue")]
        public void DetailsRouteShouldBeMapped(int id, string info)
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath($"/Fields/Details/{id}/{info}"))
                .To<FieldsController>(c => c.Details(id, info));

        [Fact]
        public void MineRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Fields/Mine"))
                .To<FieldsController>(c => c.Mine());
    }
}
