using System;
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
    class ShiftServiceTests
    {
        protected IShiftService ShiftService;
        protected Mock<IRepository<Shift>> MockShiftRepository;
        public Shift Shift;

        [SetUp]
        public void Setup()
        {
            MockShiftRepository = new Mock<IRepository<Shift>>();
            ShiftService = new ShiftService(MockShiftRepository.Object);

            Shift = new Shift()
            {
                Id = 1,
                ShiftStart = new DateTime(),
                ShiftEnd = new DateTime()
            };
        }

        [Test]
        public void ShouldGetShifts()
        {
            // ARRANGE
            var expectedShifts = new List<Shift>
            {
                new Shift() {Id = 1}
            };

            var result = new Mock<IQueryable<Shift>>();
            result.Setup(r => r.GetEnumerator()).Returns(expectedShifts.GetEnumerator());
            MockShiftRepository.Setup(x => x.Get).Returns(result.Object);

            // ACT
            var actualShift = ShiftService.Get();

            // ASSERT
            MockShiftRepository.Verify(x => x.Get, Times.Once);
            Assert.AreEqual(expectedShifts, actualShift);
        }

        [Test]
        public void ShouldGetShiftById()
        {
            // ARRANGE
            var expectedShifts = new List<Shift>
            {
                new Shift() {Id = 1}
            };

            var result = new Mock<IQueryable<Shift>>();
            result.Setup(r => r.GetEnumerator()).Returns(expectedShifts.GetEnumerator());
            MockShiftRepository.Setup(x => x.Get).Returns(result.Object);

            // ACT
            var actualShifts = ShiftService.Get();

            // ASSERT
            MockShiftRepository.Verify(x => x.Get, Times.Once);
            Assert.AreEqual(expectedShifts, actualShifts);
        }

        [Test]
        public void ShouldAddShift()
        {
            // ARRANGE
            MockShiftRepository.Setup(x => x.Add(Shift));

            // ACT
            ShiftService.Add(Shift);

            // ASSERT
            MockShiftRepository.Verify(x => x.Add(It.IsAny<Shift>()), Times.Once);
        }

        [Test]
        public void ShouldUpdateShift()
        {
            // ARRANGE
            MockShiftRepository.Setup(x => x.Update(Shift));

            // ACT
            ShiftService.Update(Shift);

            // ASSERT
            MockShiftRepository.Verify(x => x.Update(Shift), Times.Once);
        }

        [Test]
        public void ShouldRemoveShift()
        {
            MockShiftRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(new Mock<Shift>().Object);
            MockShiftRepository.Setup(x => x.Remove(It.IsAny<Shift>()));

            ShiftService.Remove(1);

            MockShiftRepository.Verify(x => x.Remove(It.IsAny<Shift>()), Times.Once());
        }
    }
}
