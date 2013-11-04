using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Zenwire.Domain;
using Zenwire.Repositories;

namespace Zenwire.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Shift> _scheduleRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Employee> _employeeRepository;

        private readonly INotificationService _notificationService;

        public AppointmentService(IRepository<Customer> customerRepository,
            IRepository<Appointment> appointmentRepository,
            IRepository<Shift> scheduleRepository,
            IRepository<Employee> employeeRepository, INotificationService notificationService)
        {
            _customerRepository = customerRepository;
            _appointmentRepository = appointmentRepository;
            _scheduleRepository = scheduleRepository;
            _employeeRepository = employeeRepository;
            _notificationService = notificationService;
        }


        public bool Schedule(Appointment appointment)
        {
            //If no employees are available in the city
            if (!IsCityAvailable(appointment))
            {
                if (appointment.Id > 0) _appointmentRepository.Update(appointment);
                else _appointmentRepository.AddOrUpdate(appointment);

                //_notificationService.SmsConfirmation(appointment);

                return true;
            }

            //If any employees are available in the city
            if (IsAvailable(appointment))
            {
                if (appointment.Id > 0) _appointmentRepository.Update(appointment);
                else _appointmentRepository.AddOrUpdate(appointment);

                // Do not uncomment unless in production
                //_notificationService.VoiceConfirmation(appointment);
                //_notificationService.SmsConfirmation(appointment);

                return true;
            }

            return false;
        }

        public Appointment Get(int id)
        {
            Appointment appointment = _appointmentRepository.Find(id);
            appointment.Customer = _customerRepository.Find(id);

            return appointment;
        }

        public bool IsCityAvailable(Appointment appointment)
        {
            var city = _customerRepository.Find(appointment.CustomerId).City ?? appointment.Customer.City;
            return _employeeRepository.Get.Count(x => x.City == city) > 0;
        }

        public bool IsAvailable(Appointment appointment)
        {
            var appointments = _appointmentRepository.Get;
            var shifts = _scheduleRepository.Get;
            var city = _customerRepository.Find(appointment.CustomerId).City ?? appointment.Customer.City;

            const int durationHour = 1;

            var inWorkingHours = shifts
                .Where(x =>
                    x.ShiftStart <= appointment.ScheduledStart &&
                    x.ShiftEnd >= appointment.ScheduledEnd &&
                    !appointments.Any(a =>
                        (appointment.Id == 0 || a.Id != appointment.Id) &&
                        a.EmployeeId == x.EmployeeId &&
                        (appointment.ScheduledStart >= a.ScheduledStart && appointment.ScheduledStart < a.ScheduledEnd) || 
                        (appointment.ScheduledEnd <= a.ScheduledEnd && appointment.ScheduledEnd > a.ScheduledStart)))
                .ToList();

            if (inWorkingHours.Any())
            {
                var assignedEmployee = inWorkingHours.FirstOrDefault().Employee;
                appointment.EmployeeId = assignedEmployee.Id;
                appointment.Employee = assignedEmployee;
                return true;
            }
            return false;
        }

        public IEnumerable<TimeSpan> GetAvailableHours(DateTime? date, int? appointmentId, int duration = 1)
        {
            if (date == null) return null;

            var hours = new List<DateTime>();
            for (var ts = new TimeSpan(); ts <= new TimeSpan(23, 30, 0); ts = ts.Add(new TimeSpan(duration, 0, 0)))
            {
                hours.Add(date.Value + ts);
            }

            var booked = _appointmentRepository.Get
                .Where(x =>
                    (!appointmentId.HasValue || x.Id != appointmentId))
                .Select(x => x.ScheduledStart).ToList();

            //return available hours from shifts
            var workingHours = hours.SelectMany(h => _scheduleRepository.Get
                .Where(x => x.ShiftStart <= h && x.ShiftEnd >= EntityFunctions.AddHours(h, 1)), (h, s) => new {h, s})
                .Where(t => t.s.ShiftStart <= t.h && t.s.ShiftEnd >= t.h.AddHours(-1) && booked.Count(x => x == t.h) == 0).Select(t => t.h.TimeOfDay);

            //match available hours with another appointment 
            return workingHours.Distinct();
        }

        public void AssignToEmployee(Appointment model)
        {
            Appointment appointment = _appointmentRepository.Find(model.Id);
            appointment.EmployeeId = model.EmployeeId;
            _appointmentRepository.AddOrUpdate(appointment);
        }

        public bool ChangeTime(int appointmentId, int days, int minutes, bool confirmUpdate = false)
        {
            Appointment appointment = _appointmentRepository.Find(appointmentId);
            appointment.ScheduledStart = appointment.ScheduledStart.AddDays(days).AddMinutes(minutes);
            if (confirmUpdate)
            {
                _appointmentRepository.AddOrUpdate(appointment);
                return true;
            }
            return Schedule(appointment);
        }

        public List<Appointment> Get()
        {
            return _appointmentRepository.Get.ToList();
        }

        public void Add(Appointment appointment)
        {

            _appointmentRepository.Add(appointment);
        }

        public void Update(Appointment appointment)
        {
            _appointmentRepository.Update(appointment);
        }

        public void Remove(int id)
        {
            Appointment appointment = Get(id);
            _appointmentRepository.Remove(appointment);
        }
    }
}