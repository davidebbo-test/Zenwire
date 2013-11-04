using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Zenwire.Domain
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
    }
}