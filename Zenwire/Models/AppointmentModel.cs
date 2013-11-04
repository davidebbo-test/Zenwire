using System;
using System.Collections.Generic;
using System.Linq;
using Zenwire.Domain;

namespace Zenwire.Models
{
    public class AppointmentModel
    {
        public string Status { get; set; }
        public Appointment Appointment { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Employee> Employees { get; set; }
        public IQueryable<Shift> Shift { get; set; }

        public AppointmentModel(Appointment appointment)
        {
            Appointment = appointment; 
        }

        public AppointmentModel()
        {
            
        }
    }
}