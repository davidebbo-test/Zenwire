using System.Web.Mvc;
using NUnit.Framework;
using Zenwire.Controllers;

namespace Tests.Unit.Zenwire.Controllers
{
    [TestFixture]
    class HomeControllerTest
    {
        protected HomeController HomeController;

        [SetUp]
        public void Setup()
        {
            HomeController = new HomeController();
        }

        [Test]
        public void IndexShouldReturnIndexView()
        {
            var view = HomeController.Index() as ViewResult;
            Assert.NotNull(view);
        }

        [Test]
        public void ServicesShouldReturnServicesView()
        {
            var view = HomeController.Services() as ViewResult;
            Assert.AreEqual("Services", view.ViewName);
            Assert.NotNull(view);
        }
    }
}
