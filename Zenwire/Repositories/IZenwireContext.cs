using System.Data.Entity;
using Zenwire.Domain;

namespace Zenwire.Repositories
{
    public interface IZenwireContext 
    {
        IDbSet<Employee> Employees { get; set; }
        IDbSet<Customer> Customers { get; set; }
        IDbSet<UserProfile> UserProfiles { get; set; }
        IDbSet<Appointment> Appointments { get; set; }
        IDbSet<Shift> Shifts { get; set; }
    }
}