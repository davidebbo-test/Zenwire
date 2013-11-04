using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zenwire.Domain;
using Zenwire.Repositories;

namespace Zenwire.Services
{
    public class ReferralService : IReferralService 
    {
        private readonly IRepository<Referral> _referralRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public ReferralService(IRepository<Referral> referralRepository, 
            IRepository<Customer> customerRepository,
            IRepository<Product> productRepository,
            IRepository<Employee> employeeRepository)
        {
            _referralRepository = referralRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _employeeRepository = employeeRepository;
        }

        public List<Referral> Get()
        {
            return _referralRepository.Get.ToList();
        }

        public Referral Get(int id)
        {
            Referral referral = _referralRepository.Find(id);
            referral.Customer = _customerRepository.Find(referral.CustomerId);
            referral.Employee = _employeeRepository.Find(referral.EmployeeId);
            
            return referral;
        }

        public decimal GetTotal(int employeeId)
        {
            List<Referral> referrals = _referralRepository.Get.Where(x => x.EmployeeId == employeeId).ToList();
            return referrals.Sum(x => x.Total);
        }

        public void Add(Referral referral)
        {
            //referral.Total = _productRepository.Find(referral.ProductId).Commission;
            _referralRepository.Add(referral);
        }

        public void Update(Referral referral)
        {
            _referralRepository.Update(referral);
        }

        public void Remove(int id)
        {
            Referral referral = Get(id);
            _referralRepository.Remove(referral);
        }
    }
}