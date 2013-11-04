using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Zenwire.Domain;
using Zenwire.Gateways;
using Zenwire.Repositories;
using Zenwire.Services;

namespace Tests.Unit.Zenwire.Services
{
    [TestFixture]
    public class AppointmentServiceTest
    {
        protected IAppointmentService AppointmentService;
        protected Mock<IRepository<Appointment>> MockAppointmentRepository;
        protected Mock<IRepository<Customer>> MockCustomerRepository;
        protected Mock<IRepository<Shift>> MockShiftRepository;
        protected Mock<IRepository<Employee>> MockEmployeeRepository;
        protected Mock<INotificationService> MockNotificationService;

        public Customer NewCustomer;
        public Appointment NewAppointment;

        [SetUp]
        public void Setup()
        {
            MockAppointmentRepository = new Mock<IRepository<Appointment>>();
            MockCustomerRepository = new Mock<IRepository<Customer>>();
            MockShiftRepository = new Mock<IRepository<Shift>>();

            MockEmployeeRepository = new Mock<IRepository<Employee>>();
            MockNotificationService = new Mock<INotificationService>();

            AppointmentService = new AppointmentService(MockCustomerRepository.Object, MockAppointmentRepository.Object,
                MockShiftRepository.Object, MockEmployeeRepository.Object, MockNotificationService.Object);

            NewCustomer = new Customer()
            {
                Address = "88 Taraview Road NE",
                City = "Calgary",
                Email = "charles.norris@outlook.com",
                FirstName = "Charles",
                LastName = "Norris",
                Id = 1,
                Phone = "587-888-8882",
                PostalCode = "X1X 1X1",
                Province = "AB"
            };

            NewAppointment = new Appointment()
            {
                Customer = new Customer() { City = "Yorkton" },
                CustomerId = 2,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 15, 12, 00, 00)
            };
        }

        [Test]
        public void IsAvailableShouldReturnFalseWhenNotWorkingHours()
        {
            var shift = new Shift()
            {
                Employee = new Mock<Employee>().Object,
                EmployeeId = 1,
                Id = 1,
                ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                ShiftEnd = new DateTime(2013, 10, 15, 18, 30, 00)
            };

            NewAppointment = new Appointment()
            {
                Customer = new Customer() { City = "Yorkton" },
                CustomerId = 2,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 16, 10, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 16, 12, 00, 00)
            };

            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(NewCustomer);
            MockShiftRepository.Setup(x => x.Get).Returns(new List<Shift> { shift }.AsQueryable());

            var isAvailable = AppointmentService.IsAvailable(NewAppointment);

            MockShiftRepository.Verify(x => x.Get, Times.Once);
            Assert.IsFalse(isAvailable);
        }

        [Test]
        public void IsCityAvailableShouldReturnTrueWhenEmployeeInSameCityFound()
        {
            var employee1 = new Employee()
            {
                Address = "12 Saddletowne Road NW",
                City = "Calgary",
                Email = "johnny.bravo@outlook.com",
                FirstName = "John",
                LastName = "Bravo",
                Id = 2,
                Phone = "403-999-2222",
                PostalCode = "X1X 1X1",
                Province = "AB"
            };

            var employee2 = new Employee()
            {
                Address = "12 Saddletowne Road NW",
                City = "Edmonton",
                Email = "johnny.bravo@outlook.com",
                FirstName = "John",
                LastName = "Bravo",
                Id = 2,
                Phone = "403-999-2222",
                PostalCode = "X1X 1X1",
                Province = "AB"
            };

            // ARRANGE
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(NewCustomer);
            MockEmployeeRepository.Setup(x => x.Get).Returns(new List<Employee> {employee1, employee2}.AsQueryable());

            // ACT
            var isAvailable = AppointmentService.IsCityAvailable(NewAppointment);

            // ASSERT
            MockCustomerRepository.Verify(x => x.Find(It.IsAny<int>()), Times.Once);
            MockEmployeeRepository.Verify(x => x.Get, Times.Once);
            Assert.IsTrue(isAvailable);
        }

        [Test]
        public void IsCityAvailableShouldReturnFalseWhenEmployeeInSameCityNotFound()
        {
            var employee2 = new Employee()
            {
                Address = "12 Saddletowne Road NW",
                City = "Edmonton",
                Email = "johnny.bravo@outlook.com",
                FirstName = "John",
                LastName = "Bravo",
                Id = 2,
                Phone = "403-999-2222",
                PostalCode = "X1X 1X1",
                Province = "AB"
            };

            // ARRANGE
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(NewCustomer);
            MockEmployeeRepository.Setup(x => x.Get).Returns(new List<Employee> {employee2}.AsQueryable());

            // ACT
            var isAvailable = AppointmentService.IsCityAvailable(NewAppointment);

            // ASSERT
            MockCustomerRepository.Verify(x => x.Find(It.IsAny<int>()), Times.Once);
            MockEmployeeRepository.Verify(x => x.Get, Times.Once);
            Assert.IsFalse(isAvailable);
        }

        [Test]
        public void ShouldScheduleAppointmentWhenIsAvailableReturnsTrue()
        {
            var customer = new Customer
            {
                City = "Calgary"
            };

            var appointment = new Appointment()
            {
                Id = 1,
                Customer = customer,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00)
            };

            var employee = new Employee
            {
                Id = 1,
                City = "Calgary"
            };

            var shift = new Shift
            {
                Employee = employee,
                EmployeeId = employee.Id,
                Id = 1,
                ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                ShiftEnd = new DateTime(2013, 10, 15, 18, 30, 00)
            };

            var employees = new List<Employee> {employee};
            var shifts = new List<Shift> {shift};

            MockEmployeeRepository.Setup(x => x.Get).Returns(employees.AsQueryable());
            MockShiftRepository.Setup(x => x.Get).Returns(shifts.AsQueryable());
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(customer);
            MockNotificationService.Setup(x => x.SmsConfirmation(appointment));
            MockAppointmentRepository.Setup(x => x.AddOrUpdate(appointment));
            MockAppointmentRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(appointment);

            AppointmentService.Schedule(appointment);

            MockAppointmentRepository.Verify(x => x.Update(It.IsAny<Appointment>()), Times.Once);
            MockAppointmentRepository.Verify(x => x.AddOrUpdate(It.IsAny<Appointment>()), Times.Never);
            //MockNotificationService.Verify(x => x.SmsConfirmation(It.IsAny<Appointment>()), Times.Once);
        }

        [Test]
        public void ShouldScheduleAppointmentWhenIsAvailableReturnsFalse()
        {
            var customer = new Customer
            {
                City = "Calgary"
            };

            var appointment = new Appointment()
            {
                Id = 0,
                Customer = customer,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00)
            };

            var employee = new Employee
            {
                Id = 1,
                City = "Calgary"
            };

            var shift = new Shift
            {
                Employee = employee,
                EmployeeId = employee.Id,
                Id = 1,
                ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                ShiftEnd = new DateTime(2013, 10, 15, 18, 30, 00)
            };

            var employees = new List<Employee> {employee};
            var shifts = new List<Shift> {shift};

            MockEmployeeRepository.Setup(x => x.Get).Returns(employees.AsQueryable());
            MockShiftRepository.Setup(x => x.Get).Returns(shifts.AsQueryable());
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(customer);
            MockNotificationService.Setup(x => x.SmsConfirmation(appointment));
            MockAppointmentRepository.Setup(x => x.AddOrUpdate(appointment));
            MockAppointmentRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(appointment);

            AppointmentService.Schedule(appointment);

            MockAppointmentRepository.Verify(x => x.AddOrUpdate(It.IsAny<Appointment>()), Times.Once);
            MockAppointmentRepository.Verify(x => x.Update(It.IsAny<Appointment>()), Times.Never);
            //MockNotificationService.Verify(x => x.SmsConfirmation(It.IsAny<Appointment>()), Times.Once);
        }

        [Test]
        public void GetAppointmentShouldReturnAppointment()
        {
            MockAppointmentRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(new Mock<Appointment>().Object);
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(new Mock<Customer>().Object);

            AppointmentService.Get(1);

            MockAppointmentRepository.Verify(x => x.Find(It.IsAny<int>()), Times.Once());
            MockCustomerRepository.Verify(x => x.Find(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void ShouldChangeTime()
        {
            var appointment = new Appointment()
            {
                Id = 1,
                Customer = new Mock<Customer>().Object,
                CustomerId = 2,
                Employee = new Mock<Employee>().Object,
                EmployeeId = 3,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00)
            };

            MockAppointmentRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(appointment);

            AppointmentService.ChangeTime(appointment.Id, 1, 0, true);

            MockAppointmentRepository.Verify(x => x.Find(It.IsAny<int>()), Times.Once);
            MockAppointmentRepository.Verify(x => x.AddOrUpdate(It.IsAny<Appointment>()), Times.Once);
        }

        [Test]
        public void ShouldGetAvailableHours()
        {
            var appointment = new Appointment()
            {
                Id = 1,
                Customer = new Mock<Customer>().Object,
                CustomerId = 2,
                Employee = new Mock<Employee>().Object,
                EmployeeId = 3,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00)
            };

            MockAppointmentRepository.Setup(x => x.Get).Returns(new List<Appointment> {appointment}.AsQueryable());
            MockShiftRepository.Setup(x => x.Get);

            AppointmentService.GetAvailableHours(new DateTime(2013, 10, 15, 10, 30, 00), null, 1);

            MockAppointmentRepository.Verify(x => x.Get, Times.Once);
        }

        [Test, Ignore] // EntityFunctions prevents us from testing GetAvailableHours any futher
        public void ShouldGetAvailableHoursForShift()
        {
            var shifts = new List<Shift>()
            {
                new Shift()
                {
                    Employee = new Employee() {Id = 1, City = "Yorkton"},
                    EmployeeId = 1,
                    Id = 1,
                    ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                    ShiftEnd = new DateTime(2013, 10, 15, 18, 30, 00)
                },
                new Shift()
              {
                    Employee = new Employee() {Id = 2, City = "Yorkton"},
                    EmployeeId = 2,
                    Id = 2,
                    ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                    ShiftEnd = new DateTime(2013, 10, 15, 14, 00, 00)
                }
            };

            var scheduledAppointments = new List<Appointment>()
            {
                new Appointment()
                {
                    Id = 1,
                    Customer = new Customer(),
                    CustomerId = 2,
                    Employee = new Employee() {Id = 1, City = "Yorkton"},
                    EmployeeId = 1,
                    ScheduledStart = new DateTime(2013, 10, 15, 10, 00, 00),
                    ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
                },
                new Appointment()
                {
                    Id = 2,
                    Customer = new Customer(),
                    CustomerId = 2,
                    Employee = new Employee() {Id = 2, City = "Yorkton"},
                    EmployeeId = 2,
                    ScheduledStart = new DateTime(2013, 10, 15, 10, 00, 00),
                    ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
                }
            };


            MockAppointmentRepository.Setup(x => x.Get).Returns(scheduledAppointments.AsQueryable());
            MockShiftRepository.Setup(x => x.Get).Returns(shifts.AsQueryable());

            var actualHours = AppointmentService.GetAvailableHours(new DateTime(2013, 10, 15), null, 1);

            MockAppointmentRepository.Verify(x => x.Get, Times.Once);
            //MockShiftRepository.Verify(x => x.Get, Times.Once);
        }

        [Test]
        public void ShouldAssignToEmployeeWhenWorkingHours()
        {
            var appointment = new Appointment()
            {
                Id = 1,
                Customer = new Mock<Customer>().Object,
                CustomerId = 2,
                Employee = new Mock<Employee>().Object,
                EmployeeId = 3,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00)
            };

            MockAppointmentRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(new Mock<Appointment>().Object);
            MockAppointmentRepository.Setup(x => x.AddOrUpdate(It.IsAny<Appointment>()));

            AppointmentService.AssignToEmployee(appointment);

            MockAppointmentRepository.Verify(x => x.Find(It.IsAny<int>()), Times.Once());
            MockAppointmentRepository.Verify(x => x.AddOrUpdate(It.IsAny<Appointment>()), Times.Once());
        }

        [Test]
        public void ShouldRemoveAppointment()
        {
            MockAppointmentRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(new Mock<Appointment>().Object);
            MockAppointmentRepository.Setup(x => x.Remove(It.IsAny<Appointment>()));

            AppointmentService.Remove(1);

            MockAppointmentRepository.Verify(x => x.Remove(It.IsAny<Appointment>()), Times.Once());
        }

        [Test]
        public void ShouldAddAppointment()
        {
            var appointment = new Appointment()
            {
                Id = 1,
                Customer = new Mock<Customer>().Object,
                CustomerId = 2,
                Employee = new Mock<Employee>().Object,
                EmployeeId = 3,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00)
            };

            MockAppointmentRepository.Setup(x => x.Add(It.IsAny<Appointment>()));

            AppointmentService.Add(appointment);

            MockAppointmentRepository.Verify(x => x.Add(It.IsAny<Appointment>()), Times.Once());
        }

        [Test]
        public void ShouldUpdateAppointment()
        {
            var appointment = new Appointment()
            {
                Id = 1,
                Customer = new Mock<Customer>().Object,
                CustomerId = 2,
                Employee = new Mock<Employee>().Object,
                EmployeeId = 3,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00)
            };

            MockAppointmentRepository.Setup(x => x.Update(It.IsAny<Appointment>()));

            AppointmentService.Update(appointment);

            MockAppointmentRepository.Verify(x => x.Update(It.IsAny<Appointment>()), Times.Once());
        }

        [Test]
        public void ShouldGetAppointments()
        {
            // ARRANGE
            var expectedAppointments = new List<Appointment>
            {
                new Appointment() {Id = 1}
            };

            var result = new Mock<IQueryable<Appointment>>();
            result.Setup(r => r.GetEnumerator()).Returns(expectedAppointments.GetEnumerator());
            MockAppointmentRepository.Setup(x => x.Get).Returns(result.Object);

            // ACT
            var actualAppointments = AppointmentService.Get();

            // ASSERT
            MockAppointmentRepository.Verify(x => x.Get, Times.Once);
            Assert.AreEqual(expectedAppointments, actualAppointments);
        }

        [Test]
        public void IsAvailableShouldReturnFalseWhenNoWorkingHoursAreAvailable()
        {
            var shifts = new List<Shift>()
            {
                new Shift()
                {
                    Employee = new Employee() {Id = 1, City = "Yorkton"},
                    EmployeeId = 1,
                    Id = 1,
                    ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                    ShiftEnd = new DateTime(2013, 10, 15, 18, 30, 00)
                },
                new Shift()
              {
                    Employee = new Employee() {Id = 2, City = "Yorkton"},
                    EmployeeId = 2,
                    Id = 2,
                    ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                    ShiftEnd = new DateTime(2013, 10, 15, 14, 00, 00)
                }
            };

            var scheduledAppointments = new List<Appointment>()
            {
                new Appointment()
                {
                    Id = 1,
                    Customer = new Customer(),
                    CustomerId = 2,
                    Employee = new Employee() {Id = 1, City = "Yorkton"},
                    EmployeeId = 1,
                    ScheduledStart = new DateTime(2013, 10, 15, 10, 00, 00),
                    ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
                },
                new Appointment()
                {
                    Id = 2,
                    Customer = new Customer(),
                    CustomerId = 2,
                    Employee = new Employee() {Id = 2, City = "Yorkton"},
                    EmployeeId = 2,
                    ScheduledStart = new DateTime(2013, 10, 15, 10, 00, 00),
                    ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
                }
            };

            var newAppointment = new Appointment()
            {
                Customer = new Customer() { City = "Yorkton" },
                CustomerId = 2,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 15, 12, 00, 00)
            };

            MockShiftRepository.Setup(x => x.Get).Returns(shifts.AsQueryable);
            MockAppointmentRepository.Setup(x => x.Get).Returns(scheduledAppointments.AsQueryable());
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(new Customer());

            Assert.IsFalse(AppointmentService.IsAvailable(newAppointment));
        }

        [Test]
        public void IsAvailableShouldReturnTrueWhenWorkingHoursAreAvailable()
        {
            var shifts = new List<Shift>()
            {
                new Shift()
                {
                    Employee = new Employee() {Id = 1, City = "Yorkton"},
                    EmployeeId = 1,
                    Id = 1,
                    ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                    ShiftEnd = new DateTime(2013, 10, 15, 18, 30, 00)
                },
                new Shift()
              {
                    Employee = new Employee() {Id = 2, City = "Yorkton"},
                    EmployeeId = 2,
                    Id = 2,
                    ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                    ShiftEnd = new DateTime(2013, 10, 15, 14, 00, 00)
                }
            };

            var scheduledAppointments = new List<Appointment>()
            {
                new Appointment()
                {
                    Id = 1,
                    Customer = new Customer(),
                    CustomerId = 2,
                    Employee = new Employee() {Id = 1, City = "Yorkton"},
                    EmployeeId = 1,
                    ScheduledStart = new DateTime(2013, 10, 15, 10, 00, 00),
                    ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
                },
                new Appointment()
                {
                    Id = 2,
                    Customer = new Customer(),
                    CustomerId = 2,
                    Employee = new Employee() {Id = 2, City = "Yorkton"},
                    EmployeeId = 2,
                    ScheduledStart = new DateTime(2013, 10, 15, 10, 00, 00),
                    ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
                }
            };

            var newAppointment = new Appointment()
            {
                Customer = new Customer() { City = "Melville" },
                CustomerId = 2,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 15, 09, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 15, 10, 00, 00)
            };

            MockShiftRepository.Setup(x => x.Get).Returns(shifts.AsQueryable);
            MockAppointmentRepository.Setup(x => x.Get).Returns(scheduledAppointments.AsQueryable());
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(new Customer());

            Assert.IsTrue(AppointmentService.IsAvailable(newAppointment));
        }

        [Test]
        public void IsAvailableShouldReturnTrueWhenNoEmployeeInCityButAppointmentIsAvailable()
        {
            var shift = new Shift()
            {
                Employee = new Employee() { Id = 1, City = "Yorkton" },
                EmployeeId = 1,
                Id = 1,
                ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                ShiftEnd = new DateTime(2013, 10, 15, 18, 30, 00)
            };

            var newAppointment = new Appointment()
            {
                Customer = new Customer() { City = "Yorkton" },
                CustomerId = 2,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 15, 08, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 15, 10, 00, 00)
            };

            var appointment = new Appointment()
            {
                Id = 1,
                Customer = new Customer() { City = "Melville" },
                CustomerId = 2,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 15, 12, 00, 00)
            };

            var customer = new Customer() { City = "Melville" };

            MockAppointmentRepository.Setup(x => x.Get).Returns(new List<Appointment> { appointment }.AsQueryable());
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(customer);
            MockShiftRepository.Setup(x => x.Get).Returns(new List<Shift> { shift }.AsQueryable());

            Assert.IsTrue(AppointmentService.IsAvailable(newAppointment));
        }

        [Test]
        public void IsAvailableShouldReturnFalseWhenNoEmployeeInCityButAppointmentIsNotAvailable()
        {
            var shift = new Shift()
            {
                Employee = new Employee() { Id = 1, City = "Yorkton" },
                EmployeeId = 1,
                Id = 1,
                ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                ShiftEnd = new DateTime(2013, 10, 15, 18, 30, 00)
            };

            var newAppointment = new Appointment()
            {
                Customer = new Customer() { City = "Yorkton" },
                CustomerId = 2,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
            };

            var appointment = new Appointment()
            {
                Id = 1,
                Customer = new Customer() { City = "Melville" },
                CustomerId = 2,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 15, 10, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 15, 12, 00, 00)
            };

            var customer = new Customer() { City = "Melville" };

            MockAppointmentRepository.Setup(x => x.Get).Returns(new List<Appointment> { appointment }.AsQueryable());
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(customer);
            MockShiftRepository.Setup(x => x.Get).Returns(new List<Shift> { shift }.AsQueryable());

            Assert.IsFalse(AppointmentService.IsAvailable(newAppointment));
        }

        [Test]
        public void IsAvailableShouldUpdateAppointmentAndReturnTrueWhenWorkingHoursAreAvailable()
        {
            var shifts = new List<Shift>()
            {
                new Shift()
                {
                    Employee = new Employee() {Id = 1, City = "Yorkton"},
                    EmployeeId = 1,
                    Id = 1,
                    ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                    ShiftEnd = new DateTime(2013, 10, 15, 18, 30, 00)
                },
                new Shift()
              {
                    Employee = new Employee() {Id = 2, City = "Yorkton"},
                    EmployeeId = 2,
                    Id = 2,
                    ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                    ShiftEnd = new DateTime(2013, 10, 15, 14, 00, 00)
                }
            };

            var scheduledAppointments = new List<Appointment>()
            {
                new Appointment()
                {
                    Id = 1,
                    Customer = new Customer(),
                    CustomerId = 2,
                    Employee = new Employee() {Id = 1, City = "Yorkton"},
                    EmployeeId = 1,
                    ScheduledStart = new DateTime(2013, 10, 15, 10, 00, 00),
                    ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
                },
                new Appointment()
                {
                    Id = 2,
                    Customer = new Customer(),
                    CustomerId = 2,
                    Employee = new Employee() {Id = 2, City = "Yorkton"},
                    EmployeeId = 2,
                    ScheduledStart = new DateTime(2013, 10, 15, 10, 00, 00),
                    ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
                }
            };

            var newAppointment = new Appointment()
            {
                Id = 3,
                Customer = new Customer() { City = "Yorkton" },
                CustomerId = 2,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 15, 09, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 15, 10, 00, 00)
            };

            MockShiftRepository.Setup(x => x.Get).Returns(shifts.AsQueryable);
            MockAppointmentRepository.Setup(x => x.Get).Returns(scheduledAppointments.AsQueryable());
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(new Customer());

            Assert.IsTrue(AppointmentService.Schedule(newAppointment));
        }

        [Test]
        public void IsAvailableShouldAddOrUpdateAppointmentAndReturnTrueWhenWorkingHoursAreAvailable()
        {
            var shifts = new List<Shift>()
            {
                new Shift()
                {
                    Employee = new Employee() {Id = 1, City = "Yorkton"},
                    EmployeeId = 1,
                    Id = 1,
                    ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                    ShiftEnd = new DateTime(2013, 10, 15, 18, 30, 00)
                },
                new Shift()
              {
                    Employee = new Employee() {Id = 2, City = "Yorkton"},
                    EmployeeId = 2,
                    Id = 2,
                    ShiftStart = new DateTime(2013, 10, 15, 08, 30, 00),
                    ShiftEnd = new DateTime(2013, 10, 15, 14, 00, 00)
                }
            };

            var scheduledAppointments = new List<Appointment>()
            {
                new Appointment()
                {
                    Id = 1,
                    Customer = new Customer(),
                    CustomerId = 2,
                    Employee = new Employee() {Id = 1, City = "Yorkton"},
                    EmployeeId = 1,
                    ScheduledStart = new DateTime(2013, 10, 15, 10, 00, 00),
                    ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
                },
                new Appointment()
                {
                    Id = 2,
                    Customer = new Customer(),
                    CustomerId = 2,
                    Employee = new Employee() {Id = 2, City = "Yorkton"},
                    EmployeeId = 2,
                    ScheduledStart = new DateTime(2013, 10, 15, 10, 00, 00),
                    ScheduledEnd = new DateTime(2013, 10, 15, 11, 00, 00)
                }
            };

            var newAppointment = new Appointment()
            {
                Customer = new Customer() { City = "Yorkton" },
                CustomerId = 2,
                Employee = null,
                EmployeeId = null,
                ScheduledStart = new DateTime(2013, 10, 15, 09, 30, 00),
                ScheduledEnd = new DateTime(2013, 10, 15, 10, 00, 00)
            };

            MockShiftRepository.Setup(x => x.Get).Returns(shifts.AsQueryable);
            MockAppointmentRepository.Setup(x => x.Get).Returns(scheduledAppointments.AsQueryable());
            MockCustomerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(new Customer());

            Assert.IsTrue(AppointmentService.Schedule(newAppointment));
        }
    }
}