using Zenwire.Domain;

namespace Zenwire.Services
{
    public interface INotificationService
    {
        void MailConfirmation(Appointment appointment);
        string VoiceConfirmation(Appointment appointment);
        string SmsConfirmation(Appointment appointment);
    }
}