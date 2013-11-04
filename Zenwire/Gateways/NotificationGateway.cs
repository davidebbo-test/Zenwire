using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Web.Configuration;
using Twilio;
using Zenwire.Domain;

namespace Zenwire.Gateways
{
    [ExcludeFromCodeCoverage]
    public class NotificationGateway : INotificationGateway
    {
        private readonly string _accountSid = WebConfigurationManager.AppSettings["AccountSid"];
        private readonly string _authToken = WebConfigurationManager.AppSettings["AuthToken"];
        private readonly string _twilioPhoneNumber = WebConfigurationManager.AppSettings["TwilioPhoneNumber"];

        private readonly SmtpClient _smtpServer;
        private readonly TwilioRestClient _twilioRestClient;

        public NotificationGateway()
        {
            _smtpServer = new SmtpClient();
            _twilioRestClient = new TwilioRestClient(_accountSid, _authToken);
        }

        public void MailConfirmation(MailMessage mailMessage)
        {
            mailMessage.IsBodyHtml = true;
            _smtpServer.Send(mailMessage);
        }

        public string VoiceConfirmation(Appointment appointment)
        {
            string voiceEndpoint = WebConfigurationManager.AppSettings["VoiceEndpoint"];

            var confirmation = _twilioRestClient.InitiateOutboundCall(
                _twilioPhoneNumber,
                appointment.Customer.Phone,
                string.Format("http://zenwire.azurewebsites.net/Notification/{0}/{1}", voiceEndpoint, appointment.Id)
                );

            return confirmation.Sid;
        }

        public string SmsConfirmation(Appointment appointment)
        {
            if (appointment.Employee.Phone != null)
            {
                var appointmentDate = appointment.ScheduledStart.ToString("dddd MMMM, d");
                var appointmentTime = appointment.ScheduledStart.ToString("hh:mm tt");

                var message = "Zenwire Appointment - Scheduled \n" + appointmentDate + " " + appointmentTime;

                var exConfirmation = _twilioRestClient.SendSmsMessage(_twilioPhoneNumber, appointment.Employee.Phone, message);

                return exConfirmation.Sid;
            }

            //if (appointment.Customer.Phone != null)
            //{
            //    var appointmentDate = appointment.ScheduledStart.ToString("dddd MMMM, d");
            //    var appointmentTime = appointment.ScheduledStart.ToString("hh:mm tt");

            //    var message = "Zenwire Appointment - Scheduled \n" + appointmentDate + " " + appointmentTime;

            //    var exConfirmation = _twilioRestClient.SendSmsMessage(_twilioPhoneNumber, appointment.Customer.Phone, message);

            //    return exConfirmation.Sid;
            //}

            return null;
        }
    }
}