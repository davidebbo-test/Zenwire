using System.Collections.Generic;
using System.Linq;
using Zenwire.Domain;
using Zenwire.Domain.Commissions;
using Zenwire.Repositories;

namespace Zenwire.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<PayCode> _payCodeRepository;

        public EmployeeService(IRepository<Employee> employeeRepository, IRepository<PayCode> payCodeRepository)
        {
            _employeeRepository = employeeRepository;
            _payCodeRepository = payCodeRepository;
        }

        public Employee Get(int id)
        {
            return _employeeRepository.Get.ToList().FirstOrDefault(x => x.Id == id);
        }

        public List<Employee> Get()
        {
            return _employeeRepository.Get.ToList();
        }

        public int Add(Employee employee)
        {
            var existing = _employeeRepository.Get.ToList().FirstOrDefault(x => x.Email == employee.Email);

            if (existing == null) _employeeRepository.Add(employee);
            return employee.Id;
        }

        public void Update(Employee employee)
        {
            _employeeRepository.Update(employee);
        }

        public void Remove(int id)
        {
            Employee employee = Get(id);
            _employeeRepository.Remove(employee);
        }

        // TODO: Should move to own service?
        public List<PayCode> GetPayCodes()
        {
            return _payCodeRepository.Get.ToList();
        }

        public PayCode GetPayCodeById(int id)
        {
            return _payCodeRepository.Get.ToList().FirstOrDefault(x => x.Id == id);
        } 
    }
}