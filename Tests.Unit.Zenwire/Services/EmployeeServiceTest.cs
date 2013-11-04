using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Zenwire.Domain;
using Zenwire.Domain.Commissions;
using Zenwire.Repositories;
using Zenwire.Services;

namespace Tests.Unit.Zenwire.Services
{
    [TestFixture]
    class EmployeeServiceTests
    {
        protected IEmployeeService EmployeeService;
        protected Mock<IRepository<Employee>> MockEmployeeRepository;
        protected Mock<IRepository<PayCode>> MockPayCodeRepository;
        public Employee Employee;

        [SetUp]
        public void Setup()
        {
            MockEmployeeRepository = new Mock<IRepository<Employee>>();
            EmployeeService = new EmployeeService(MockEmployeeRepository.Object, MockPayCodeRepository.Object);

            Employee = new Employee()
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
        public void ShouldGetEmployees()
        {
            // ARRANGE
            var expectedEmployees = new List<Employee>
            {
                new Employee() {Id = 1}
            };

            var result = new Mock<IQueryable<Employee>>();
            result.Setup(r => r.GetEnumerator()).Returns(expectedEmployees.GetEnumerator());
            MockEmployeeRepository.Setup(x => x.Get).Returns(result.Object);

            // ACT
            var actualEmployee = EmployeeService.Get();

            // ASSERT
            MockEmployeeRepository.Verify(x => x.Get, Times.Once);
            Assert.AreEqual(expectedEmployees, actualEmployee);
        }

        [Test]
        public void ShouldGetEmployeeById()
        {
            // ARRANGE
            var Employees = new List<Employee>
            {
                new Employee() {Id = 1}
            };

            var result = new Mock<IQueryable<Employee>>();
            result.Setup(r => r.GetEnumerator()).Returns(Employees.GetEnumerator());
            MockEmployeeRepository.Setup(x => x.Get).Returns(result.Object);

            // ACT
            var Employee = EmployeeService.Get(1);

            // ASSERT
            MockEmployeeRepository.Verify(x => x.Get, Times.Once);
            Assert.AreEqual(1, Employee.Id);
        }

        [Test]
        public void ShouldAddEmployee()
        {
            // ARRANGE
            var employees = new List<Employee>
            {
                Employee
            };

            var result = new Mock<IQueryable<Employee>>();
            result.Setup(r => r.GetEnumerator()).Returns(employees.GetEnumerator());
            MockEmployeeRepository.Setup(x => x.Get).Returns(result.Object);
            MockEmployeeRepository.Setup(x => x.Add(It.IsAny<Employee>()));

            // ACT
            EmployeeService.Add(new Employee());

            // ASSERT
            MockEmployeeRepository.Verify(x => x.Get, Times.Once);
            MockEmployeeRepository.Verify(x => x.Add(It.IsAny<Employee>()), Times.Once);
        }

        [Test]
        public void ShouldUpdateEmployee()
        {
            // ARRANGE
            MockEmployeeRepository.Setup(x => x.Update(Employee));

            // ACT
            EmployeeService.Update(Employee);

            // ASSERT
            MockEmployeeRepository.Verify(x => x.Update(Employee), Times.Once);
        }

        [Test]
        public void ShouldRemoveEmployee()
        {
            // ARRANGE
            var Employees = new List<Employee>
            {
                new Employee() {Id = 1}
            };

            var result = new Mock<IQueryable<Employee>>();
            result.Setup(r => r.GetEnumerator()).Returns(Employees.GetEnumerator());
            MockEmployeeRepository.Setup(x => x.Get).Returns(result.Object);
            MockEmployeeRepository.Setup(x => x.Remove(It.IsAny<Employee>()));

            // ACT
            var Employee = EmployeeService.Get(1);
            EmployeeService.Remove(Employee.Id);

            // ASSERT
            MockEmployeeRepository.Verify(x => x.Remove(It.IsAny<Employee>()), Times.Once);
        } 
    }
}
