using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SatyamHealthCare.Controllers;
using SatyamHealthCare.DTO;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Exceptions;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.Repos;

namespace SatyamHealthCare.Tests.Controllers
{
    [TestFixture]
    public class DoctorsControllerTests
    {
        private Mock<IDoctor> _mockDoctorRepo;
        private SatyamDbContext _context;
        private DoctorsController _controller;

        [SetUp]
        public void Setup()
        {
            // Mock the repository
            _mockDoctorRepo = new Mock<IDoctor>();

            // Set up an in-memory database
            var options = new DbContextOptionsBuilder<SatyamDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new SatyamDbContext(options);

            // Create the controller
            _controller = new DoctorsController(_context, _mockDoctorRepo.Object);
        }

        [Test]
        public async Task GetDoctors_ReturnsOk_WithListOfDoctors()
        {
            // Arrange
            _context.Doctors.Add(new Doctor { DoctorId = 1, FullName = "Dr. Smith", PhoneNo = "123456789", Email = "dr.smith@example.com", Password = "password", Designation = "Surgeon", Experience = 5, SpecializationID = 1, Qualification = "MD" });
            _context.Doctors.Add(new Doctor { DoctorId = 2, FullName = "Dr. Jones", PhoneNo = "987654321", Email = "dr.jones@example.com", Password = "password", Designation = "Pediatrician", Experience = 7, SpecializationID = 2, Qualification = "MBBS" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetDoctors();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var doctors = okResult.Value as IEnumerable<DoctorDTO>;
            Assert.IsNotNull(doctors);
            Assert.AreEqual(2, doctors.Count());
        }


        [Test]
        public async Task GetDoctor_ExistingId_ReturnsOk_WithDoctor()
        {
            // Arrange
            var doctor = new Doctor { DoctorId = 1, FullName = "Dr. Smith" };
            _mockDoctorRepo.Setup(repo => repo.GetDoctorById(1)).ReturnsAsync(doctor);

            // Act
            var result = await _controller.GetDoctor(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(doctor, okResult.Value);
        }

        [Test]
        public async Task GetDoctor_NonExistingId_ThrowsDoctorNotFoundException()
        {
            // Arrange
            _mockDoctorRepo.Setup(repo => repo.GetDoctorById(It.IsAny<int>())).ReturnsAsync((Doctor)null);

            // Act & Assert
            Assert.ThrowsAsync<DoctorNotFoundException>(async () => await _controller.GetDoctor(999));
        }

        [Test]
        public async Task DeleteDoctor_ExistingId_ReturnsOk()
        {
            // Arrange
            var doctor = new Doctor { DoctorId = 1 };
            _mockDoctorRepo.Setup(repo => repo.GetDoctorById(1)).ReturnsAsync(doctor);

            // Act
            var result = await _controller.DeleteDoctor(1);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            _mockDoctorRepo.Verify(repo => repo.DeleteDoctor(1), Times.Once);
        }

        [Test]
        public async Task DeleteDoctor_NonExistingId_ThrowsDoctorNotFoundException()
        {
            // Arrange
            _mockDoctorRepo.Setup(repo => repo.GetDoctorById(It.IsAny<int>())).ReturnsAsync((Doctor)null);

            // Act & Assert
            Assert.ThrowsAsync<DoctorNotFoundException>(async () => await _controller.DeleteDoctor(999));
        }

        [TearDown]
        public void TearDown()
        {
            // Clear the in-memory database
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
