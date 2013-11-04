using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Zenwire.Controllers;
using Zenwire.Domain;
using Zenwire.Models;
using Zenwire.Services;

namespace Tests.Unit.Zenwire.Controllers
{
    [TestFixture]
    public class AppointmentControllerTest
    {
        protected Mock<IAppointmentService> MockAppointmentService;
        protected Mock<ICustomerService> MockCustomerService;
        protected Mock<IEmployeeService> MockEmployeeService;
        protected AppointmentController AppointmentController;

        [SetUp]
        public void Setup()
        {
            MockAppointmentService = new Mock<IAppointmentService>();
            MockCustomerService = new Mock<ICustomerService>();
            MockEmployeeService = new Mock<IEmployeeService>();

            AppointmentController = new AppointmentController(MockAppointmentService.Object, MockCustomerService.Object, MockEmployeeService.Object);
        }

        [Test]
        public void ShouldScheduleAppointmentWhenModelStateValid()
        {
            // ACT
            AppointmentController.Schedule(new ScheduleModel());

            // ASSERT
            MockCustomerService.Verify(x => x.GetByEmail(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ShouldNotScheduleAppointmentWhenModelStateInValid()
        {
            // ACT
            AppointmentController.ModelState.AddModelError("key", "error message");
            AppointmentController.Schedule(new ScheduleModel());

            // ASSERT
            MockCustomerService.Verify(x => x.GetByEmail(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void IndexShouldReturnAppointmentList()
        {
            AppointmentController.Index();
            MockAppointmentService.Verify(x => x.Get(), Times.Once);
        }

        [Test]
        public void ThankYouShouldReturnCorrectView()
        {
            var view = AppointmentController.ThankYou() as ViewResult;
            Assert.NotNull(view);
            Assert.AreEqual("ThankYou", view.ViewName);
        }

        [Test]
        public void ScheduleShouldReturnCorrectView()
        {
            var view = AppointmentController.Schedule() as ViewResult;
            Assert.NotNull(view);
            Assert.AreEqual("Schedule", view.ViewName);
            //Assert.AreEqual(typeof(ScheduleModel), view.Model);
        }

        [Test]
        public void DetailsShouldReturnAppointmentWhenAppointmentNotNull()
        {
            // ARRANGE
            var expectedAppointment = new Appointment()
            {
                Id = 1,
            };

            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedAppointment);

            // ACT
            AppointmentController.Details(expectedAppointment.Id);

            // ASSERT
            MockAppointmentService.Verify(x => x.Get(It.Is<int>(y => y == expectedAppointment.Id)), Times.Once);

        }

        [Test]
        public void DetailsShouldNotFindCustomerWhenAppointmentNull()
        {
            // ARRANGE
            Appointment expectedAppointment = null;

            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedAppointment);

            // ACT
            var result = AppointmentController.Details(1) as HttpNotFoundResult;

            // ASSERT
            MockAppointmentService.Verify(x => x.Get(It.Is<int>(y => y == 1)), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteConfirmedShouldRemoveAppointment()
        {
            // ARRANGE
            var expectedAppointment = new Appointment()
            {
                Id = 1,
            };

            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedAppointment);

            // ACT
            AppointmentController.DeleteConfirmed(1);

            // ASSERT
            MockAppointmentService.Verify(x => x.Remove(It.Is<int>(y => y == expectedAppointment.Id)), Times.Once);
        }

        [Test]
        public void DeleteShouldReturnHttpNotFoundWhenAppointmentIsNull()
        {
            // ARRANGE
            Appointment expectedAppointment = null;

            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedAppointment);
            MockAppointmentService.Setup(x => x.Remove(It.IsAny<int>()));

            // ACT
            var result = AppointmentController.Delete(1) as HttpNotFoundResult;

            // ASSERT
            MockAppointmentService.Verify(x => x.Remove(It.Is<int>(y => y == expectedAppointment.Id)), Times.Never);
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteShouldReturnConfirmationDetailsWhenAppointmentIsNotNull()
        {
            // ARRANGE
            var expectedAppointment = new Appointment()
            {
                Id = 1,
            };

            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedAppointment);

            // ACT
            var result = AppointmentController.Delete(1) as ViewResult;

            // ASSERT
            MockAppointmentService.Verify(x => x.Get(It.Is<int>(y => y == expectedAppointment.Id)), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void CreateShouldReturnAppointmentCreateView()
        {
            var view = AppointmentController.Create() as ViewResult;
            Assert.NotNull(view);
        }

        [Test]
        public void CreateShouldAddAppointmentWhenAppointmentIsNotNull()
        {
            // ARRANGE
            var expectedAppointment = new Appointment()
            {
                Id = 1,
                Customer = new Customer(),
                CustomerId = 2,
                Employee = new Employee(),
                ScheduledStart = new DateTime()
            };

            var fakeAppointmentModel = new AppointmentModel(expectedAppointment);

            MockAppointmentService.Setup(x => x.Add(It.IsAny<Appointment>()));

            // ACT
            AppointmentController.Create(fakeAppointmentModel);

            // ASSERT
            MockAppointmentService.Verify(x => x.Schedule(It.Is<Appointment>(y => y == expectedAppointment)), Times.Once);
        }

        [Test]
        public void CreateShouldNotAddOrUpdateCustomerWhenModelStateInvalid()
        {
            // ARRANGE
            var expectedAppointment = new Appointment()
            {
                Id = 1,
                Customer = new Customer(),
                CustomerId = 2,
                Employee = new Employee(),
                ScheduledStart = new DateTime()
            };

            var fakeAppointmentModel = new AppointmentModel(expectedAppointment);

            // ACT
            AppointmentController.ModelState.AddModelError("key", "error message");
            AppointmentController.Create(fakeAppointmentModel);

            // ASSERT
            MockAppointmentService.Verify(x => x.Add(It.Is<Appointment>(y => y == expectedAppointment)), Times.Never);

        }

        [Test]
        public void EditShouldCallRepositoryAndReturnAppointment()
        {
            // ARRANGE
            var expectedAppointment = new Appointment()
            {
                Id = 1,
                Customer = new Customer(),
                CustomerId = 2,
                Employee = new Employee(),
                ScheduledStart = new DateTime()
            };

            var fakeAppointmentModel = new AppointmentModel(expectedAppointment);

            //var mockContext = new Mock<ZenwireContext>();
            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedAppointment);

            // ACT
            var result = AppointmentController.Edit(1) as ViewResult;
            var resultData = (AppointmentModel)result.ViewData.Model;

            // ASSERT
            MockAppointmentService.Verify(x => x.Get(It.Is<int>(y => y == expectedAppointment.Id)), Times.Once);
        }

        [Test]
        public void EditShouldReturnHttpNotFoundWhenAppointmentNull()
        {
            // ARRANGE
            Appointment expectedAppointment = null;

            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedAppointment);

            // ACT
            var result = AppointmentController.Edit(1) as HttpNotFoundResult;

            // ASSERT
            Assert.NotNull(result);
        }

        [Test]
        public void EditPostShouldScheduleAppointmentWhenModelStateValid()
        {
            // ARRANGE
            var expectedAppointment = new Appointment()
            {
                Id = 1,
                Customer = new Customer(),
                CustomerId = 0,
                Employee = new Employee(),
                ScheduledStart = new DateTime()
            };

            var fakeAppointmentModel = new AppointmentModel(expectedAppointment);

            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedAppointment);

            // ACT
            var result = AppointmentController.Edit(fakeAppointmentModel) as ViewResult;

            // ASSERT
            MockAppointmentService.Verify(x => x.Schedule(It.Is<Appointment>(y => y == expectedAppointment)), Times.Once);
            MockAppointmentService.Verify(x => x.Get(It.Is<int>(y => y == expectedAppointment.Id)), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void EditPostShouldNotScheduleAppointmentWhenModelStateInvalid()
        {
            // ARRANGE
            var expectedAppointment = new Appointment()
            {
                Id = 1,
                Customer = new Customer(),
                CustomerId = 2,
                Employee = new Employee(),
                ScheduledStart = new DateTime()
            };

            var fakeAppointmentModel = new AppointmentModel(expectedAppointment);

            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedAppointment);

            // ACT
            AppointmentController.ModelState.AddModelError("key", "error message");
            var result = AppointmentController.Edit(fakeAppointmentModel) as ViewResult;

            // ASSERT
            MockAppointmentService.Verify(x => x.Schedule(It.Is<Appointment>(y => y == expectedAppointment)), Times.Never);
            Assert.NotNull(result);
        }

        [Test]
        public void AssignPostShouldAssignAppointmentWhenModelStateIsValid()
        {
            // ARRANGE
            var expectedAppointment = new Appointment()
            {
                Id = 1,
                Customer = new Customer(),
                CustomerId = 2,
                Employee = new Employee(),
                ScheduledStart = new DateTime()
            };

            var fakeAppointmentModel = new AppointmentModel(expectedAppointment);

            // ACT
            MockAppointmentService.Setup(x => x.AssignToEmployee(It.IsAny<Appointment>()));
            AppointmentController.Assign(fakeAppointmentModel);

            // ASSERT
            MockAppointmentService.Verify(x => x.AssignToEmployee(It.Is<Appointment>(y => y == expectedAppointment)), Times.Once);
        }

        [Test]
        public void ShouldNotAssignWhenAppointmentIsNull()
        {
            // ARRANGE
            Appointment expectedAppointment = null;

            // ACT
            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(expectedAppointment);
            var result = AppointmentController.Assign(0) as HttpNotFoundResult;

            // ASSERT
            MockAppointmentService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void CreateAppointmentShouldScheduleAppointmentWhenCustomerIsNotNull()
        {
            var customer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                PostalCode = "X1X 1X1",
                Province = "AB"
            };

            var appointment = new Appointment()
            {
                Customer = customer,
                CustomerId = customer.Id,
                Id = 1,
                ScheduledStart = new DateTime(2013, 10, 15, 18, 30, 00)
            };

            var scheduleModel = new ScheduleModel()
                {
                    Appointment = appointment,
                    Customer = customer
                };

            MockCustomerService.Setup(x => x.Add(It.IsAny<Customer>())).Returns(customer);
            AppointmentController.CreateAppointment(scheduleModel);

            MockAppointmentService.Verify(x => x.Schedule(It.Is<Appointment>(y => y == appointment)), Times.Once);
        }

        [Test]
        public void CreateAppointmentShouldNotScheduleAppointmentWhenCustomerIsNull()
        {
            Customer customer = null;

            var appointment = new Appointment()
            {
                Id = 1,
                ScheduledStart = new DateTime(2013, 10, 15, 18, 30, 00)
            };

            var scheduleModel = new ScheduleModel()
            {
                Appointment = appointment
            };

            MockCustomerService.Setup(x => x.Add(It.IsAny<Customer>())).Returns(customer);
            AppointmentController.CreateAppointment(scheduleModel);

            MockAppointmentService.Verify(x => x.Schedule(It.Is<Appointment>(y => y == appointment)), Times.Never);
        }

        [Test]
        public void CreateAppointmentShouldNotScheduleAppointmentWhenModelStateIsNotValid()
        {
            Customer customer = null;

            var appointment = new Appointment()
            {
                Id = 1,
                ScheduledStart = new DateTime(2013, 10, 15, 18, 30, 00)
            };

            var scheduleModel = new ScheduleModel()
            {
                Appointment = appointment
            };

            MockCustomerService.Setup(x => x.Add(It.IsAny<Customer>())).Returns(customer);
            AppointmentController.ModelState.AddModelError("Error", "Message");
            var result = AppointmentController.CreateAppointment(scheduleModel) as HttpNotFoundResult;


            MockAppointmentService.Verify(x => x.Schedule(It.Is<Appointment>(y => y == appointment)), Times.Never);
            Assert.NotNull(result);
        }

        [Test]
        public void ChangeTimeShouldReturnIsSuccess()
        {
            var appointment = new Appointment()
            {
                Id = 1,
                ScheduledStart = new DateTime(2013, 10, 15, 18, 30, 00)
            };

            MockAppointmentService.Setup(x => x.ChangeTime(appointment.Id, 1, 30, false));
            AppointmentController.ChangeTime(1, 1, 30, false);

            MockAppointmentService.Verify(x => x.ChangeTime(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), false), Times.Once);
        }

        [Test]
        public void ChangeTimeShouldConfirmUpdate()
        {
            var appointment = new Appointment()
            {
                Id = 1,
                ScheduledStart = new DateTime(2013, 10, 15, 18, 30, 00)
            };

            MockAppointmentService.Setup(x => x.ChangeTime(appointment.Id, 1, 30, true));
            AppointmentController.ChangeTime(1, 1, 30, true);

            MockAppointmentService.Verify(x => x.ChangeTime(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), true), Times.Once);
        }

        [Test]
        public void GetEventsShouldReturnEvents()
        {
            var cx = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                PostalCode = "X1X 1X1",
                Province = "AB"
            };

            var appointment = new Appointment()
            {
                Customer = cx,
                CustomerId = cx.Id,
                Id = 1,
                ScheduledStart = DateTime.Now.ToUniversalTime()
            };

            MockAppointmentService.Setup(x => x.Get()).Returns(new List<Appointment> { appointment });
            var result = AppointmentController.GetEvents(1380434400, 1384066800);
            var actualResult = ((IEnumerable<dynamic>)result.Data).ToList();
            var item = actualResult[0];

            var T = item.GetType();
            var id = T.GetProperty("id").GetValue(item, null).ToString();
            var title = T.GetProperty("title").GetValue(item, null).ToString();
            var address = T.GetProperty("address").GetValue(item, null).ToString();
            var city = T.GetProperty("city").GetValue(item, null).ToString();
            var phone = T.GetProperty("phone").GetValue(item, null).ToString();
            var start = T.GetProperty("start").GetValue(item, null).ToString();
            var end = T.GetProperty("end").GetValue(item, null).ToString();

            var culture = new CultureInfo("en-CA");

            MockAppointmentService.Verify(x => x.Get(), Times.Once);
            Assert.AreEqual("1", id);
            Assert.AreEqual("Unassigned", title);
            Assert.AreEqual("88 Taraview Road NE", address);
            Assert.AreEqual("Calgary", city);
            Assert.AreEqual("587-888-8882", phone);

            var expectedStartTime = appointment.ScheduledStart.ToUniversalTime().ToString("r", culture);
            var expectedEndTime = appointment.ScheduledStart.AddHours(1).ToUniversalTime().ToString("r", culture);
            // The returned times are UTC formatted without timezone, we convert to the correct timezone
            // elsewhere. We will look for a better approach to doing this in the future.
            Assert.AreEqual(expectedStartTime, start);
            Assert.AreEqual(expectedEndTime, end);
        }
    }
}