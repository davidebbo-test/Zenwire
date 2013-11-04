using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Zenwire.Domain;
using Zenwire.Repositories;
using Zenwire.Services;

namespace Tests.Unit.Zenwire.Services
{
    [TestFixture]
    class CustomerServiceTests
    {
        protected ICustomerService CustomerService;
        protected Mock<IRepository<Customer>> MockCustomerRepository;
        public Customer Customer;

        [SetUp]
        public void Setup()
        {
            MockCustomerRepository = new Mock<IRepository<Customer>>();
            CustomerService = new CustomerService(MockCustomerRepository.Object);

            Customer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                Province = "AB"
            };
        }

        [Test]
        public void ShouldGetCustomers()
        {
            // ARRANGE
            var expectedCustomers = new List<Customer>
            {
                new Customer() {Id = 1}
            };

            var result = new Mock<IQueryable<Customer>>();
            result.Setup(r => r.GetEnumerator()).Returns(expectedCustomers.GetEnumerator());
            MockCustomerRepository.Setup(x => x.Get).Returns(result.Object);

            // ACT
            var actualCustomer = CustomerService.Get();

            // ASSERT
            MockCustomerRepository.Verify(x => x.Get, Times.Once);
            Assert.AreEqual(expectedCustomers, actualCustomer);
        }

        [Test]
        public void ShouldGetCustomerById()
        {
            // ARRANGE
            var customers = new List<Customer>
            {
                new Customer() {Id = 1}
            };

            var result = new Mock<IQueryable<Customer>>();
            result.Setup(r => r.GetEnumerator()).Returns(customers.GetEnumerator());
            MockCustomerRepository.Setup(x => x.Get).Returns(result.Object);

            // ACT
            var customer = CustomerService.Get(1);

            // ASSERT
            MockCustomerRepository.Verify(x => x.Get, Times.Once);
            Assert.AreEqual(1, customer.Id);
        }

        [Test]
        public void ShouldGetCustomerByEmail()
        {
            // ARRANGE
            var customers = new List<Customer>
            {
                new Customer() {Id = 1, Email = "charles.norris@outlook.com"}
            };

            var result = new Mock<IQueryable<Customer>>();
            result.Setup(r => r.GetEnumerator()).Returns(customers.GetEnumerator());
            MockCustomerRepository.Setup(x => x.Get).Returns(result.Object);

            // ACT
            var customer = CustomerService.GetByEmail("charles.norris@outlook.com");

            // ASSERT
            MockCustomerRepository.Verify(x => x.Get, Times.Once);
            Assert.AreEqual("charles.norris@outlook.com", customer.Email);
        }

        [Test]
        public void ShouldAddCustomerWhenNotExisting()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charlie.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 3,
                Phone = "587-888-8882",
                Province = "AB"
            };

            var customers = new List<Customer>
            {
                new Customer() { Id = 2, Email = "jonh.bravo@outlook.com"}  
            };

            var result = new Mock<IQueryable<Customer>>();
            result.Setup(r => r.GetEnumerator()).Returns(customers.GetEnumerator());
            MockCustomerRepository.Setup(x => x.Get).Returns(result.Object);

            MockCustomerRepository.Setup(x => x.Add(expectedCustomer));

            // ACT
            var actualCustomer = CustomerService.Add(expectedCustomer);

            // ASSERT
            MockCustomerRepository.Verify(x => x.Get, Times.Once);
            MockCustomerRepository.Verify(x => x.Add(It.IsAny<Customer>()), Times.Once);

            Assert.AreEqual(expectedCustomer, actualCustomer);
        }

        [Test]
        public void ShouldNotAddCustomerWhenExisting()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 2,
                Phone = "587-888-8882",
                Province = "AB"
            };

            var customer = new Customer()
            {
                Address = "11842 72 Street",
                City = "Edmonton",
                Email = "bruce.lee@outlook.com",
                FirstName = "Bruce",
                LastName = "Lee",
                Id = 4,
                Phone = "403-888-1112",
                Province = "AB"
            };

            var customers = new List<Customer>
            {
                customer,
                expectedCustomer
            };

            var result = new Mock<IQueryable<Customer>>();
            result.Setup(r => r.GetEnumerator()).Returns(customers.GetEnumerator());
            MockCustomerRepository.Setup(x => x.Get).Returns(result.Object);

            // ACT
            var actualCustomer = CustomerService.Add(expectedCustomer);

            // ASSERT
            MockCustomerRepository.Verify(x => x.Get, Times.Once);
            MockCustomerRepository.Verify(x => x.Add(It.IsAny<Customer>()), Times.Never);

            Assert.AreEqual(expectedCustomer, actualCustomer);
        }

        [Test]
        public void ShouldReturnNullIfCustomerIdNotValid()
        {
            // ARRANGE
            var expectedCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 0,
                Phone = "587-888-8882",
                Province = "AB"
            };

            var customers = new List<Customer>
            {
                new Customer()
                {
                    Id = 1,
                    Email = "not.found@outlook.com"
                }
            };

            var result = new Mock<IQueryable<Customer>>();
            result.Setup(r => r.GetEnumerator()).Returns(customers.GetEnumerator());
            MockCustomerRepository.Setup(x => x.Get).Returns(result.Object);
            MockCustomerRepository.Setup(x => x.Add(expectedCustomer));

            // ACT
            var actualCustomer = CustomerService.Add(expectedCustomer);

            // ASSERT
            MockCustomerRepository.Verify(x => x.Get, Times.Once);
            MockCustomerRepository.Verify(x => x.Add(It.IsAny<Customer>()), Times.AtLeastOnce);

            Assert.IsNull(actualCustomer);
        }

        [Test]
        public void ShouldUpdateCustomer()
        {
            // ARRANGE
            MockCustomerRepository.Setup(x => x.Update(Customer));

            // ACT
            CustomerService.Update(Customer);

            // ASSERT
            MockCustomerRepository.Verify(x => x.Update(Customer), Times.Once);
        }

        [Test]
        public void ShouldRemoveCustomer()
        {
            // ARRANGE
            var customers = new List<Customer>
            {
                new Customer() {Id = 1}
            };

            var result = new Mock<IQueryable<Customer>>();
            result.Setup(r => r.GetEnumerator()).Returns(customers.GetEnumerator());
            MockCustomerRepository.Setup(x => x.Get).Returns(result.Object);
            MockCustomerRepository.Setup(x => x.Remove(Customer));

            // ACT
            var customer = CustomerService.Get(1);
            CustomerService.Remove(Customer.Id);

            // ASSERT
            MockCustomerRepository.Verify(x => x.Remove(It.IsAny<Customer>()), Times.Once);
        }
    }
}
