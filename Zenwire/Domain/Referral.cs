using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zenwire.Domain.Commissions;

namespace Zenwire.Domain
{
    public class Referral
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public decimal Total { get; set; }

        public virtual List<PayCode> PayCodes { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual int PayCode { get; set; }
    }
}