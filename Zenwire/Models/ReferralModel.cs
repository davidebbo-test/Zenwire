using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zenwire.Domain;
using Zenwire.Domain.Commissions;

namespace Zenwire.Models
{
    public class ReferralModel
    {
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public List<PayCode> PayCodes { get; set; }
        public List<PayCode> PayCodesList { get; set; }
        public string SelectedPayCode { get; set; }

        public Referral Referral { get; set; }

        public ReferralModel()
        {
            PayCodesList = new List<PayCode>();
        }

        public void Add(PayCode payCode)
        {
            PayCodesList.Add(payCode);
        }
    }
}