using System;
using System.Collections.Generic;
using Zenwire.Domain;

namespace Zenwire.Services
{
    public interface IAppointmentService
    {
        bool IsAvailable(Appointment appointment);
        bool IsCityAvailable(Appointment appointment);
        Appointment Get(int id);
        List<Appointment> Get();
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        void Remove(int id);

        bool Schedule(Appointment appointment);
        IEnumerable<TimeSpan> GetAvailableHours(DateTime? date, int? appointmentId, int duration);
        void AssignToEmployee(Appointment model);
        bool ChangeTime(int appointmentId, int days, int minutes, bool confirmUpdate = false);
    }
}