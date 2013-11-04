using System.Net.Mail;
using Zenwire.Domain;

namespace Zenwire.Gateways
{
    public interface INotificationGateway
    {
        void MailConfirmation(MailMessage mailMessage);
        string VoiceConfirmation(Appointment appointment);
        string SmsConfirmation(Appointment appointment);
    }
}