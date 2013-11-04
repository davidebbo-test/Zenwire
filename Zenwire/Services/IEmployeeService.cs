using System.Collections.Generic;
using Zenwire.Domain;
using Zenwire.Domain.Commissions;

namespace Zenwire.Services
{
    public interface IEmployeeService
    {
        Employee Get(int id);
        List<Employee> Get();
        int Add(Employee employee);
        void Update(Employee employee);
        void Remove(int id);

        List<PayCode> GetPayCodes();
        PayCode GetPayCodeById(int id);
    }
}