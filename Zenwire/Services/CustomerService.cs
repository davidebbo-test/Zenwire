using System.Collections.Generic;
using System.Linq;
using Zenwire.Domain;
using Zenwire.Repositories;

namespace Zenwire.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Customer Get(int id)
        {
            return _customerRepository.Get.ToList().FirstOrDefault(x => x.Id == id);
        }

        public List<Customer> Get()
        {
            return _customerRepository.Get.ToList();
        }

        public Customer GetByEmail(string email)
        {
            return _customerRepository.Get.ToList().FirstOrDefault(x => x.Email == email);
        }

        public Customer Add(Customer customer)
        {
            var existing = _customerRepository.Get.ToList().FirstOrDefault(x => x.Email == customer.Email);
            
            if (existing != null)
            {
                customer = existing;
                return customer;
            }

            _customerRepository.Add(customer);
            return customer.Id > 0 ? customer : null;

            //var cx = _customerRepository.Get.ToList().FirstOrDefault(x => x.Email == customer.Email);
            //if (cx != null)
            //{
            //    customer = cx;
            //    return customer;
            //}
        }

        public void Update(Customer customer)
        {
            _customerRepository.Update(customer);
        }

        public void Remove(int id)
        {
            Customer customer = Get(id);
            _customerRepository.Remove(customer);
        }
    }
}