using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using Zenwire.Domain;
using Zenwire.Models;
using Zenwire.Domain.Commissions;

namespace Zenwire.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ZenwireContext : DbContext , IDisposable
    {
        public ZenwireContext()
            : base("name=Repositories.ZenwireContext")
        {
            
        }

        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<UserProfile> UserProfiles { get; set; }
        public IDbSet<Appointment> Appointments { get; set; }
        public IDbSet<Shift> Shifts { get; set; }

        public IDbSet<PayCode> PayCodes { get; set; }

    }
}

