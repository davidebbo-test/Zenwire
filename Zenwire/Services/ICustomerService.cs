using System.Collections.Generic;
using Zenwire.Domain;

namespace Zenwire.Services
{
    public interface ICustomerService
    {
        Customer GetByEmail(string email);
        Customer Get(int id);
        List<Customer> Get();
        Customer Add(Customer customer);
        void Update(Customer customer);
        void Remove(int id);
    }
}