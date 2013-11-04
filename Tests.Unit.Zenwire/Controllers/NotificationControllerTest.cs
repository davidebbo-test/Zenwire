using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Zenwire.Controllers;
using Zenwire.Domain;
using Zenwire.Repositories;
using Zenwire.Services;

namespace Tests.Unit.Zenwire.Controllers
{
    [TestFixture]
    class NotificationControllerTest
    {
        protected Mock<IAppointmentService> MockAppointmentService;
        protected NotificationController NotificationController;

        public Customer Customer;
        public Appointment Appointment;

        [SetUp]
        public void Setup()
        {
            MockAppointmentService = new Mock<IAppointmentService>();
            NotificationController = new NotificationController(MockAppointmentService.Object);

            Customer = new Customer()
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

            Appointment = new Appointment()
            {
                Customer = Customer,
                CustomerId = Customer.Id,
                Id = 1,
                ScheduledStart = new DateTime(2013, 10, 15, 18, 30, 00)
            };
        }

        [Test]
        public void AppointmentConfirmationShouldReturnValidResponse()
        {
            var expectedContent = @"<Response>
  <Say voice=""man"" loop=""2"">Hello. This is an automated call from ZEN WIRE, your consultation appointment for Tuesday, , October, , 15, , at 6, , 30, ,P, M has been confirmed!</Say>
  <Pause length=""1"" />
  <Say>We look forward to seeing you! Have a great day!</Say>
  <Pause length=""2"" />
</Response>";

            MockAppointmentService.Setup(x => x.Get(1)).Returns(Appointment);
            
            var actualResponse = NotificationController.AppointmentConfirmation(1);


            Assert.AreEqual(expectedContent, actualResponse.Content);
            MockAppointmentService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void AppointmentConfirmationShouldReturnNullIfAppointmentIsNull()
        {
            Appointment appointment = null;
            MockAppointmentService.Setup(x => x.Get(It.IsAny<int>())).Returns(appointment);

            var actualResponse = NotificationController.AppointmentConfirmation(0);


            Assert.IsNull(actualResponse);
            MockAppointmentService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }
    }
}
