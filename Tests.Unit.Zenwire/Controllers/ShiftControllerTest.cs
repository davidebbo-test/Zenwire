using System;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Zenwire.Controllers;
using Zenwire.Domain;
using Zenwire.Models;
using Zenwire.Repositories;

namespace Tests.Unit.Zenwire.Controllers
{
    [TestFixture]
    public class ShiftControllerTest
    {
        protected Mock<IRepository<Shift>> MockShiftRepository;
        protected Mock<IRepository<Employee>> MockEmployeeRepository;
        protected ShiftController ShiftController;

        public Employee FakeEmployee;
        public ShiftModel FakeShiftModel;

        [SetUp]
        public void Setup()
        {
            MockShiftRepository = new Mock<IRepository<Shift>>();
            MockEmployeeRepository = new Mock<IRepository<Employee>>();
            ShiftController = new ShiftController(MockEmployeeRepository.Object, MockShiftRepository.Object);

            FakeEmployee = new Employee()
            {
                Address = "34 Greyhill Drive NW",
                City = "Edmonton",
                PostalCode = "T7X 1M3",
                Province = "AB",
                Email = "ronburgendy@company.com",
                Phone = "111-222-8888",
                FirstName = "Ron",
                LastName = "Burgendy",
                Id = 1
            };

            FakeShiftModel  = new ShiftModel()
            {
                EmployeeList = null,
                ShiftEntity =
                    new Shift()
                    {
                        Employee = FakeEmployee,
                        EmployeeId = FakeEmployee.Id,
                        ShiftStart = new DateTime(2013, 10, 01, 08, 00, 00),
                        ShiftEnd = new DateTime(2013, 10, 01, 18, 00, 00)
                    }
            };
        }

        [Test]
        public void IndexShouldReturnShiftList()
        {
            ShiftController.Index();
            MockShiftRepository.Verify(x => x.Get, Times.Once);
        }

        [Test]
        public void CreateShouldReturnShiftCreateView()
        {
            var view = ShiftController.Create() as ViewResult;
            Assert.NotNull(view);
        }

        [Test]
        public void CreateShouldAddShiftWhenShiftIsNotNull()
        {
            // ARRANGE
            MockShiftRepository.Setup(x => x.Add(It.IsAny<Shift>()));

            // ACT
            ShiftController.Create(FakeShiftModel, new DateTime());

            // ASSERT
            MockShiftRepository.Verify(x => x.Add(It.Is<Shift>(y => y == FakeShiftModel.ShiftEntity)), Times.Once);
        }

        [Test]
        public void CreateShouldNotAddShiftWhenShiftIsNull()
        {
            // ARRANGE
            FakeShiftModel.ShiftEntity = null;

            MockShiftRepository.Setup(x => x.Add(It.IsAny<Shift>()));

            // ACT
            var result = ShiftController.Details(1) as HttpNotFoundResult;

            // ASSERT
            MockShiftRepository.Verify(x => x.Add(It.Is<Shift>(y => y == FakeShiftModel.ShiftEntity)), Times.Never);
            Assert.NotNull(result);
        }

        [Test]
        public void CreateShouldNotAddOrUpdateShiftWhenModelStateInvalid()
        {
            // ARRANGE


            // ACT
            ShiftController.ModelState.AddModelError("key", "error message");
            ShiftController.Create(FakeShiftModel, new DateTime());

            // ASSERT
            MockShiftRepository.Verify(x => x.Add(It.Is<Shift>(y => y == FakeShiftModel.ShiftEntity)), Times.Never);

        }

        [Test]
        public void DetailsShouldReturnShiftWhenShiftNotNull()
        {
            // ARRANGE
            MockShiftRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(FakeShiftModel.ShiftEntity);

            // ACT
            ShiftController.Details(FakeShiftModel.ShiftEntity.Id);

            // ASSERT
            MockShiftRepository.Verify(x => x.Find(It.Is<int>(y => y == FakeShiftModel.ShiftEntity.Id)), Times.Once);

        }

        [Test]
        public void DetailsShouldNotFindShiftWhenShiftNull()
        {
            // ARRANGE
            FakeShiftModel.ShiftEntity = null;

            MockShiftRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(FakeShiftModel.ShiftEntity);

            // ACT
            var result = ShiftController.Details(1) as HttpNotFoundResult;

            // ASSERT
            MockShiftRepository.Verify(x => x.Find(It.Is<int>(y => y == 1)), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteConfirmedShouldRemoveShift()
        {
            // ARRANGE
            MockShiftRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(FakeShiftModel.ShiftEntity);
            MockShiftRepository.Setup(x => x.Remove(It.IsAny<Shift>()));

            // ACT
            ShiftController.DeleteConfirmed(FakeShiftModel.ShiftEntity.Id);

            // ASSERT
            MockShiftRepository.Verify(x => x.Remove(It.Is<Shift>(y => y == FakeShiftModel.ShiftEntity)), Times.Once);

        }

        [Test]
        public void DeleteShouldReturnHttpNotFoundWhenShiftIsNull()
        {
            // ARRANGE
            FakeShiftModel.ShiftEntity = null;

            MockShiftRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(FakeShiftModel.ShiftEntity);
            MockShiftRepository.Setup(x => x.Remove(It.IsAny<Shift>()));

            // ACT
            var result = ShiftController.Delete(1) as HttpNotFoundResult;

            // ASSERT
            MockShiftRepository.Verify(x => x.Remove(It.Is<Shift>(y => y == FakeShiftModel.ShiftEntity)), Times.Never);
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteShouldReturnConfirmationDetailsWhenShiftIsNotNull()
        {
            // ARRANGE
            MockShiftRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(FakeShiftModel.ShiftEntity);

            // ACT
            var result = ShiftController.Delete(1) as ViewResult;

            // ASSERT
            MockShiftRepository.Verify(x => x.Find(It.Is<int>(y => y == 1)), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void EditShouldCallRepositoryAndReturnShift()
        {
            // ARRANGE
            MockShiftRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(FakeShiftModel.ShiftEntity);

            // ACT
            var result = ShiftController.Edit(1) as ViewResult;
            var resultData = (ShiftModel) result.ViewData.Model;

            // ASSERT
            Assert.NotNull(resultData);
        }

        [Test]
        public void EditShouldReturnHttpNotFoundWhenShiftNull()
        {
            // ARRANGE
            FakeShiftModel.ShiftEntity = null;

            //var mockContext = new Mock<ZenwireContext>();
            MockShiftRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(FakeShiftModel.ShiftEntity);

            // ACT
            var result = ShiftController.Edit(1) as HttpNotFoundResult;

            // ASSERT
            Assert.NotNull(result);
        }

        [Test]
        public void EditPostShouldAddOrUpdateShiftWhenModelStateValid()
        {
            // ARRANGE
            MockShiftRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(FakeShiftModel.ShiftEntity);

            // ACT
            ShiftController.Edit(FakeShiftModel);

            // ASSERT
            MockShiftRepository.Verify(x => x.Update(It.Is<Shift>(y => y == FakeShiftModel.ShiftEntity)), Times.Once);

        }

        [Test]
        public void EditPostShouldNotAddOrUpdateShiftWhenModelStateInvalid()
        {
            // ARRANGE
            MockShiftRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(FakeShiftModel.ShiftEntity);

            // ACT
            ShiftController.ModelState.AddModelError("key", "error message");
            ShiftController.Edit(FakeShiftModel);

            // ASSERT
            MockShiftRepository.Verify(x => x.AddOrUpdate(It.Is<Shift>(y => y == FakeShiftModel.ShiftEntity)), Times.Never);

        }
    }
}