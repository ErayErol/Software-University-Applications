//namespace MiniFootball.Test.Controllers
//{
//    using MiniFootball.Controllers;
//    using Models.Fields;
//    using MyTested.AspNetCore.Mvc;
//    using Services.Fields;
//    using Xunit;

//    public class FieldsControllerTest
//    {
//        [Fact]
//        public void AllShouldReturnView()
//            => MyController<FieldsController>
//                .Instance()
//                .Calling(c => c.All(new FieldAllQueryModel()))
//                .ShouldReturn()
//                .View();

//        [Fact]
//        public void GetCreateShouldBeForAuthorizedUsers()
//            => MyController<FieldsController>
//                .Instance()
//                .Calling(c => c.Create(new FieldCreateFormModel()))
//                .ShouldHave()
//                .ActionAttributes(attributes => attributes
//                    .RestrictingForAuthorizedRequests())
//                .AndAlso()
//                .ShouldReturn()
//                .Redirect(redirect => redirect
//                    .To<AdminsController>(c => c.Become()));

//        [Fact]
//        public void GetEditShouldBeForAuthorizedUsers()
//            => MyController<FieldsController>
//                .Instance()
//                .Calling(c => c.Edit(new FieldEditFormModel()))
//                .ShouldHave()
//                .ActionAttributes(attributes => attributes
//                    .RestrictingForAuthorizedRequests())
//                .AndAlso()
//                .ShouldReturn()
//                .Redirect(redirect => redirect
//                    .To<AdminsController>(c => c.Become()));

//        [Fact]
//        public void GetDeleteShouldBeForAuthorizedUsers()
//            => MyController<FieldsController>
//                .Instance()
//                .Calling(c => c.Delete(new FieldDeleteServiceModel()))
//                .ShouldHave()
//                .ActionAttributes(attributes => attributes
//                    .RestrictingForAuthorizedRequests())
//                .AndAlso()
//                .ShouldReturn()
//                .Redirect(redirect => redirect
//                    .To<AdminsController>(c => c.Become()));

//        //[Fact]
//        //public void GetDetailsShouldBeForAuthorizedUsers()
//        //    => MyController<FieldsController>
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
//            => MyController<FieldsController>
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
