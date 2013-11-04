using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Zenwire.Controllers;
using Zenwire.Domain;
using Zenwire.Services;

namespace Tests.Unit.Zenwire.Controllers
{
    [TestFixture]
    public class CustomerControllerTest
    {
        protected Mock<ICustomerService> MockCustomerService;
        protected CustomerController CustomerController;

        [SetUp]
        public void Setup()
        {
            MockCustomerService = new Mock<ICustomerService>();
            CustomerController = new CustomerController(MockCustomerService.Object);
        }

        [Test]
        public void IndexShouldReturnCustomerList()
        {
            CustomerController.Index();
            MockCustomerService.Verify(x => x.Get(), Times.Once);
        }

        [Test]
        public void CreateShouldReturnCustomerCreateView()
        {
            var view = CustomerController.Create() as ViewResult;
            Assert.NotNull(view);
        }

        [Test]
        public void CreateShouldNotAddOrUpdateCustomerWhenModelStateValid()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                Province = "AB"
            };

            MockCustomerService.Setup(x => x.Add(It.IsAny<Customer>()));

            // ACT
            CustomerController.Create(expectedCustomer);

            // ASSERT
            MockCustomerService.Verify(x => x.Add(It.Is<Customer>(y => y == expectedCustomer)), Times.Once);
        }

        [Test]
        public void CreateShouldNotAddOrUpdateCustomerWhenModelStateInvalid()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                Province = "AB"
            };

            // ACT
            CustomerController.ModelState.AddModelError("key", "error message");
            CustomerController.Create(expectedCustomer);

            // ASSERT
            MockCustomerService.Verify(x => x.Add(It.Is<Customer>(y => y == expectedCustomer)), Times.Never);

        }

        [Test]
        public void DetailsShouldReturnCustomerWhenCustomerNotNull()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                Province = "AB"
            };

            MockCustomerService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedCustomer);

            // ACT
            CustomerController.Details(expectedCustomer.Id);

            // ASSERT
            MockCustomerService.Verify(x => x.Get(It.Is<int>(y => y == expectedCustomer.Id)), Times.Once);

        }

        [Test]
        public void DetailsShouldNotFindCustomerWhenCustomerNull()
        {
            // ARRANGE
            Customer expectedCustomer = null;


            MockCustomerService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedCustomer);

            // ACT
            var result = CustomerController.Details(1) as HttpNotFoundResult;

            // ASSERT
            MockCustomerService.Verify(x => x.Get(It.Is<int>(y => y == 1)), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteConfirmedShouldRemoveCustomer()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                Province = "AB"
            };

            MockCustomerService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedCustomer);
            MockCustomerService.Setup(x => x.Remove(It.IsAny<int>()));

            // ACT
            CustomerController.DeleteConfirmed(expectedCustomer.Id);

            // ASSERT
            MockCustomerService.Verify(x => x.Remove(It.Is<int>(y => y == expectedCustomer.Id)), Times.Once);

        }

        [Test]
        public void DeleteShouldReturnHttpNotFoundWhenCustomerIsNull()
        {
            // ARRANGE
            Customer expectedCustomer = null;

            MockCustomerService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedCustomer);
            MockCustomerService.Setup(x => x.Remove(It.IsAny<int>()));

            // ACT
            var result = CustomerController.Delete(1) as HttpNotFoundResult;

            // ASSERT
            MockCustomerService.Verify(x => x.Remove(It.Is<int>(y => y == expectedCustomer.Id)), Times.Never);
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteShouldReturnConfirmationDetailsWhenCustomerIsNotNull()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                Province = "AB"
            };

            MockCustomerService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedCustomer);

            // ACT
            var result = CustomerController.Delete(1) as ViewResult;

            // ASSERT
            MockCustomerService.Verify(x => x.Get(It.Is<int>(y => y == expectedCustomer.Id)), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void EditShouldCallRepositoryAndReturnCustomer()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                Province = "AB"
            };

            //var mockContext = new Mock<ZenwireContext>();
            MockCustomerService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedCustomer);

            // ACT
            var result = CustomerController.Edit(1) as ViewResult;
            var resultData = (Customer) result.ViewData.Model;

            // ASSERT
            Assert.AreEqual(expectedCustomer, resultData);
        }

        [Test]
        public void EditShouldReturnHttpNotFoundWhenCustomerNull()
        {
            // ARRANGE
            Customer expectedCustomer = null;

            //var mockContext = new Mock<ZenwireContext>();
            MockCustomerService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedCustomer);

            // ACT
            var result = CustomerController.Edit(1) as HttpNotFoundResult;

            // ASSERT
            Assert.NotNull(result);
        }

        [Test]
        public void EditPostShouldAddOrUpdateCustomerWhenModelStateValid()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                Province = "AB"
            };

            // ACT
            CustomerController.Edit(expectedCustomer);

            // ASSERT
            MockCustomerService.Verify(x => x.Update(It.Is<Customer>(y => y == expectedCustomer)), Times.Once);

        }

        [Test]
        public void EditPostShouldNotAddOrUpdateCustomerWhenModelStateInvalid()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                Province = "AB"
            };

            // ACT
            CustomerController.ModelState.AddModelError("key", "error message");
            CustomerController.Edit(expectedCustomer);

            // ASSERT
            MockCustomerService.Verify(x => x.Update(It.Is<Customer>(y => y == expectedCustomer)), Times.Never);

        }
    }
}