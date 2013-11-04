using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Twilio;
using Zenwire.Domain;
using Zenwire.Gateways;

namespace Zenwire.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationGateway _notificationGateway;

        public NotificationService(INotificationGateway notificationGateway)
        {
            _notificationGateway = notificationGateway;
        }

        public void MailConfirmation(Appointment appointment)
        {
            MailMessage mailMessage = new MailMessage();
            Customer customer = appointment.Customer;

            mailMessage.To.Add(customer.Email);
            mailMessage.Subject = "Zenwire - Appointment Confirmation";
            mailMessage.Body = "Hello " + customer.LastName + ", " + customer.FirstName +
                               "<br><br>We have confirmed your appointment for " +
                               appointment.ScheduledStart.ToString(("dddd MMMM d, yyyy h:mm tt") + ".") +
                               "<br><br>If you have any questions or wish to reschedule this appointment please contact us." +
                               "<br><br>" +
                               "Thank you," +
                               "<br>Zenwire";

            mailMessage.IsBodyHtml = true;

            _notificationGateway.MailConfirmation(mailMessage);
        }

        public string VoiceConfirmation(Appointment appointment)
        {
            return _notificationGateway.VoiceConfirmation(appointment);
        }

        public string SmsConfirmation(Appointment appointment)
        {
            return _notificationGateway.SmsConfirmation(appointment);
        }
    }
}