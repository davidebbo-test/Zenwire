using System.Linq;
using Zenwire.Domain;

namespace Zenwire.Models
{
    public class ShiftModel
    {
        public Shift ShiftEntity { get; set; }
        public IQueryable<Employee> EmployeeList { get; set; }
    }
}