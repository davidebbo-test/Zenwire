using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Ajax.Utilities;
using Zenwire.Domain;
namespace Zenwire.Models
{
    public class ScheduleModel
    {
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string CustomerSearch { get; set; }
        public Customer Customer { get; set; }
        public Appointment Appointment { get; set; } 
        public TimeSpan AppointmentTime { get; set; }
    }
}