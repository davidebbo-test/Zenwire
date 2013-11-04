using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Zenwire.Domain;
using Zenwire.Gateways;
using Zenwire.Services;

namespace Tests.Unit.Zenwire.Services
{
    [TestFixture]
    class NotificationServiceTest
    {
        protected Mock<INotificationGateway> NotificationGatewayMock;
        protected INotificationService NotificationService;

        public Customer NewCustomer;
        public Appointment NewAppointment;

        [SetUp]
        public void SetUp()
        {
            NotificationGatewayMock = new Mock<INotificationGateway>();
            NotificationService = new NotificationService(NotificationGatewayMock.Object);

            NewCustomer = new Customer()
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

            NewAppointment = new Appointment()
            {
                Customer = NewCustomer,
                CustomerId = 1,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 15, 12, 00, 00)
            };
        }
            
        [Test]
        public void ShouldSendMailConfirmation()
        {
            NotificationGatewayMock.Setup(x => x.MailConfirmation(It.IsAny<MailMessage>()));
            
            NotificationService.MailConfirmation(NewAppointment);

            NotificationGatewayMock.Verify(x => x.MailConfirmation(It.IsAny<MailMessage>()));
        }

        [Test]
        public void ShouldSendSmsConfirmation()
        {
            NotificationGatewayMock.Setup(x => x.SmsConfirmation(NewAppointment));

            NotificationService.SmsConfirmation(NewAppointment);

            NotificationGatewayMock.Verify(x => x.SmsConfirmation(It.Is<Appointment>(y => y == NewAppointment)), Times.Once);
        }

        [Test]
        public void ShouldSendVoiceConfirmation()
        {
            NotificationGatewayMock.Setup(x => x.VoiceConfirmation(NewAppointment));

            NotificationService.VoiceConfirmation(NewAppointment);

            NotificationGatewayMock.Verify(x => x.VoiceConfirmation(It.Is<Appointment>(y => y == NewAppointment)), Times.Once);
        }
    }
}
