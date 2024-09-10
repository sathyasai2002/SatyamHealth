using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.Controllers;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SatyamHealthCare.Tests.Controllers
{
    [TestFixture]
    public class DoctorsControllerTests
    {
        private Mock<IDoctor> _mockDoctorRepo;
        private DoctorsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockDoctorRepo = new Mock<IDoctor>();
            _controller = new DoctorsController(null, _mockDoctorRepo.Object);
        }

        // Unit Test for GetDoctors
        [Test]
        public async Task GetDoctors_ReturnsOkResult_WithListOfDoctors()
        {
            // Arrange
            var specialization = new Specialization { SpecializationID = 1, SpecializationName = "Cardiology" };
            var specialization1 = new Specialization { SpecializationID = 2, SpecializationName = "Dermatology" };

            var doctors = new List<Doctor>
            {
                new Doctor { DoctorId = 1, FullName = "Doctor1", Specialization =specialization },
                new Doctor { DoctorId = 2, FullName = "Doctor2", Specialization = specialization1 }
            };
            _mockDoctorRepo.Setup(repo => repo.GetAllDoctors()).ReturnsAsync(doctors);

            // Act
            var result = await _controller.GetDoctors();

            // Assert
            Assert.IsInstanceOf<ActionResult<IEnumerable<Doctor>>>(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var resultDoctors = okResult.Value as IEnumerable<Doctor>;

            Assert.AreEqual(doctors.Count, resultDoctors?.Count());
        }

        // Unit Test for GetDoctor (Doctor Exists)
        [Test]
        public async Task GetDoctor_ReturnsOk_WhenDoctorExists()
        {
            // Arrange
            var specialization = new Specialization { SpecializationID = 1, SpecializationName = "Cardiology" };

            var doctor = new Doctor { DoctorId = 1, FullName = "Doctor1", Specialization = specialization };
            _mockDoctorRepo.Setup(repo => repo.GetDoctorById(1)).ReturnsAsync(doctor);

            // Act
            var result = await _controller.GetDoctor(1);

            // Assert
            Assert.IsInstanceOf<ActionResult<Doctor>>(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(doctor, okResult?.Value);
        }

        // Unit Test for GetDoctor (Doctor Does Not Exist)
        [Test]
        public async Task GetDoctor_ReturnsNotFound_WhenDoctorDoesNotExist()
        {
            // Arrange
            _mockDoctorRepo.Setup(repo => repo.GetDoctorById(1)).ReturnsAsync((Doctor)null);

            // Act
            var result = await _controller.GetDoctor(1);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        // Unit Test for PostDoctor (Successful Creation)
        [Test]
        public async Task PostDoctor_ReturnsCreatedAtAction_WhenDoctorIsCreated()
        {
            // Arrange
            var specialization = new Specialization { SpecializationID = 1, SpecializationName = "Cardiology" };

            var doctor = new Doctor { DoctorId = 1, FullName = "Doctor1", Specialization = specialization };

            _mockDoctorRepo.Setup(repo => repo.AddDoctor(It.IsAny<Doctor>())).ReturnsAsync(doctor);

            // Act
            var result = await _controller.PostDoctor(doctor);

            // Assert
            Assert.IsInstanceOf<ActionResult<Doctor>>(result);
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.AreEqual(nameof(_controller.PostDoctor), createdAtActionResult.ActionName);
            Assert.AreEqual(doctor.DoctorId, ((Doctor)createdAtActionResult.Value).DoctorId);
        }

        // Unit Test for PostDoctor (Invalid Model State)
     
        
    }
}
