using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Zenwire.Domain;

namespace Tests.Unit.Zenwire.Domain
{
    [TestFixture]
    class EmployeeTest
    {
        [Test]
        public void EmployeeFullNameShouldConcatCorrectly()
        {
            var employee = new Employee()
            {
                Address = "12 Saddletowne Road NW",
                City = "Calgary",
                Email = "johnny.bravo@outlook.com",
                FirstName = "Tim",
                LastName = "Horton",
                Id = 2,
                Phone = "403-999-2222",
                PostalCode = "X1X 1X1",
                Province = "AB"
            };

            Assert.AreEqual("Tim Horton", employee.Fullname);
        }
    }
}
