using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zenwire.Domain.Commissions
{
    public class PayCode
    {
        [Key]
        public int Id { get; set; }

        public int Code { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
    }
}