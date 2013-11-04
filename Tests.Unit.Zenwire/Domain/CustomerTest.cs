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
    class CustomerTest
    {
        [Test]
        public void CustomerFullNameShouldConcatCorrectly()
        {
            var customer = new Customer()
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

            Assert.AreEqual("John Bravo", customer.Fullname);
        }
    }
}
