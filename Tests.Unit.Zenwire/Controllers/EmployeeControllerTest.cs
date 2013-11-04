using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Zenwire.Controllers;
using Zenwire.Domain;
using Zenwire.Services;

namespace Tests.Unit.Zenwire.Controllers
{
    [TestFixture]
    public class EmployeeControllerTest
    {
        protected Mock<IEmployeeService> MockEmployeeService;
        protected EmployeeController EmployeeController;

        [SetUp]
        public void Setup()
        {
            MockEmployeeService = new Mock<IEmployeeService>();
            EmployeeController = new EmployeeController(MockEmployeeService.Object);
        }

        [Test]
        public void IndexShouldReturnEmployeeList()
        {
            EmployeeController.Index();
            MockEmployeeService.Verify(x => x.Get(), Times.Once);
        }

        [Test]
        public void CreateShouldReturnEmployeeCreateView()
        {
            var view = EmployeeController.Create() as ViewResult;
            Assert.NotNull(view);
        }

        [Test]
        public void CreateShouldAddEmployeeWhenEmployeeIsNotNull()
        {
            // ARRANGE
            var expectedEmployee = new Employee()
            {
                Address = "81 Galaxy Way NW",
                City = "Calgary",
                Email = "george.denver@company.com",
                FirstName = "George",
                LastName = "Denver",
                Id = 1,
                Phone = "881-888-1111",
                Province = "AB"
            };

            MockEmployeeService.Setup(x => x.Add(It.IsAny<Employee>()));

            // ACT
            EmployeeController.Create(expectedEmployee);

            // ASSERT
            MockEmployeeService.Verify(x => x.Add(It.Is<Employee>(y => y == expectedEmployee)), Times.Once);
        }

        [Test]
        public void CreateShouldNotAddEmployeeWhenEmployeeIsNull()
        {
            // ARRANGE
            Employee expectedEmployee = null;


            MockEmployeeService.Setup(x => x.Add(It.IsAny<Employee>()));

            // ACT
            var result = EmployeeController.Details(1) as HttpNotFoundResult;

            // ASSERT
            MockEmployeeService.Verify(x => x.Add(It.Is<Employee>(y => y == expectedEmployee)), Times.Never);
            Assert.NotNull(result);
        }

        [Test]
        public void CreateShouldNotAddOrUpdateEmployeeWhenModelStateInvalid()
        {
            // ARRANGE
            var expectedEmployee = new Employee()
            {
                Address = "81 Galaxy Way NW",
                City = "Calgary",
                Email = "george.denver@company.com",
                FirstName = "George",
                LastName = "Denver",
                Id = 1,
                Phone = "881-888-1111",
                Province = "AB"
            };

            // ACT
            EmployeeController.ModelState.AddModelError("key", "error message");
            EmployeeController.Create(expectedEmployee);

            // ASSERT
            MockEmployeeService.Verify(x => x.Add(It.Is<Employee>(y => y == expectedEmployee)), Times.Never);

        }

        [Test]
        public void DetailsShouldReturnEmployeeWhenEmployeeNotNull()
        {
            // ARRANGE
            var expectedEmployee = new Employee()
            {
                Address = "81 Galaxy Way NW",
                City = "Calgary",
                Email = "george.denver@company.com",
                FirstName = "George",
                LastName = "Denver",
                Id = 1,
                Phone = "881-888-1111",
                Province = "AB"
            };

            MockEmployeeService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedEmployee);

            // ACT
            EmployeeController.Details(expectedEmployee.Id);

            // ASSERT
            MockEmployeeService.Verify(x => x.Get(It.Is<int>(y => y == expectedEmployee.Id)), Times.Once);

        }

        [Test]
        public void DetailsShouldNotFindEmployeeWhenEmployeeNull()
        {
            // ARRANGE
            Employee expectedEmployee = null;


            MockEmployeeService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedEmployee);

            // ACT
            var result = EmployeeController.Details(1) as HttpNotFoundResult;

            // ASSERT
            MockEmployeeService.Verify(x => x.Get(It.Is<int>(y => y == 1)), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteConfirmedShouldRemoveEmployee()
        {
            // ARRANGE
            var expectedEmployee = new Employee()
            {
                Address = "81 Galaxy Way NW",
                City = "Calgary",
                Email = "george.denver@company.com",
                FirstName = "George",
                LastName = "Denver",
                Id = 1,
                Phone = "881-888-1111",
                Province = "AB"
            };

            MockEmployeeService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedEmployee);
            MockEmployeeService.Setup(x => x.Remove(It.IsAny<int>()));

            // ACT
            EmployeeController.DeleteConfirmed(expectedEmployee.Id);

            // ASSERT
            MockEmployeeService.Verify(x => x.Remove(It.Is<int>(y => y == expectedEmployee.Id)), Times.Once);

        }

        [Test]
        public void DeleteShouldReturnHttpNotFoundWhenEmployeeIsNull()
        {
            // ARRANGE
            Employee expectedEmployee = null;

            MockEmployeeService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedEmployee);
            MockEmployeeService.Setup(x => x.Remove(It.IsAny<int>()));

            // ACT
            var result = EmployeeController.Delete(1) as HttpNotFoundResult;

            // ASSERT
            MockEmployeeService.Verify(x => x.Remove(It.Is<int>(y => y == expectedEmployee.Id)), Times.Never);
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteShouldReturnConfirmationDetailsWhenEmployeeIsNotNull()
        {
            // ARRANGE
            var expectedEmployee = new Employee()
            {
                Address = "81 Galaxy Way NW",
                City = "Calgary",
                Email = "george.denver@company.com",
                FirstName = "George",
                LastName = "Denver",
                Id = 1,
                Phone = "881-888-1111",
                Province = "AB"
            };

            MockEmployeeService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedEmployee);

            // ACT
            var result = EmployeeController.Delete(1) as ViewResult;

            // ASSERT
            MockEmployeeService.Verify(x => x.Get(It.Is<int>(y => y == expectedEmployee.Id)), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void EditShouldCallRepositoryAndReturnEmployee()
        {
            // ARRANGE
            var expectedEmployee = new Employee()
            {
                Address = "81 Galaxy Way NW",
                City = "Calgary",
                Email = "george.denver@company.com",
                FirstName = "George",
                LastName = "Denver",
                Id = 1,
                Phone = "881-888-1111",
                Province = "AB"
            };

            //var mockContext = new Mock<ZenwireContext>();
            MockEmployeeService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedEmployee);

            // ACT
            var result = EmployeeController.Edit(1) as ViewResult;
            var resultData = (Employee) result.ViewData.Model;

            // ASSERT
            Assert.AreEqual(expectedEmployee, resultData);
        }

        [Test]
        public void EditShouldReturnHttpNotFoundWhenEmployeeNull()
        {
            // ARRANGE
            Employee expectedEmployee = null;

            //var mockContext = new Mock<ZenwireContext>();
            MockEmployeeService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedEmployee);

            // ACT
            var result = EmployeeController.Edit(1) as HttpNotFoundResult;

            // ASSERT
            Assert.NotNull(result);
        }

        [Test]
        public void EditPostShouldAddOrUpdateEmployeeWhenModelStateValid()
        {
            // ARRANGE
            var expectedEmployee = new Employee()
            {
                Address = "81 Galaxy Way NW",
                City = "Calgary",
                Email = "george.denver@company.com",
                FirstName = "George",
                LastName = "Denver",
                Id = 1,
                Phone = "881-888-1111",
                Province = "AB"
            };

            // ACT
            EmployeeController.Edit(expectedEmployee);

            // ASSERT
            MockEmployeeService.Verify(x => x.Update(It.Is<Employee>(y => y == expectedEmployee)), Times.Once);

        }

        [Test]
        public void EditPostShouldNotAddOrUpdateEmployeeWhenModelStateInvalid()
        {
            // ARRANGE
            var expectedEmployee = new Employee()
            {
                Address = "81 Galaxy Way NW",
                City = "Calgary",
                Email = "george.denver@company.com",
                FirstName = "George",
                LastName = "Denver",
                Id = 1,
                Phone = "881-888-1111",
                Province = "AB"
            };

            // ACT
            EmployeeController.ModelState.AddModelError("key", "error message");
            EmployeeController.Edit(expectedEmployee);

            // ASSERT
            MockEmployeeService.Verify(x => x.Update(It.Is<Employee>(y => y == expectedEmployee)), Times.Never);

        }
    }
}