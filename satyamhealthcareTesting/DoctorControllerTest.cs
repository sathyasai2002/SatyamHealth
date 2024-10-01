using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.Controllers;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.DTO;
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
                new Doctor { DoctorId = 1, FullName = "Doctor1", Specialization = specialization },
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
        
        [Test]
        public async Task PostDoctor_ReturnsOk_WhenDoctorIsCreated()
        {
            // Arrange
            var doctorDto = new DoctorDTO
            {
                FullName = "Doctor1",
                PhoneNo = "1234567890",
                Email = "doctor1@example.com",
                Password = "Password123",
                Designation = "Cardiologist",
                Experience = 5,
                SpecializationID = 1,
                Qualification = "MBBS",
                AdminId = 1
            };

            var doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Doctor1",
                PhoneNo = "1234567890",
                Email = "doctor1@example.com",
                Password = "Password123",
                Designation = "Cardiologist",
                Experience = 5,
                SpecializationID = 1,
                Qualification = "MBBS",
               // ProfilePicture = null,
                AdminId = 1
            };

            _mockDoctorRepo.Setup(repo => repo.AddDoctor(It.IsAny<Doctor>())).ReturnsAsync(doctor);
            _mockDoctorRepo.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostDoctor(doctorDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result, "Result is not OkObjectResult");
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult, "OkObjectResult is null");

            // Use JObject to parse the response
            var response = JObject.FromObject(okResult.Value);

            Assert.IsNotNull(response, "Response object is null");

            // Check message
            Assert.AreEqual("Doctor registered successfully", response["message"].ToString());

            // Check doctor object in response
            var doctorResult = response["doctor"].ToObject<Doctor>();
            Assert.IsNotNull(doctorResult, "Doctor object in response is null");

            Assert.AreEqual(doctor.DoctorId, doctorResult.DoctorId);
            Assert.AreEqual(doctor.FullName, doctorResult.FullName);
            Assert.AreEqual(doctor.PhoneNo, doctorResult.PhoneNo);
            Assert.AreEqual(doctor.Email, doctorResult.Email);
            Assert.AreEqual(doctor.Designation, doctorResult.Designation);
            Assert.AreEqual(doctor.Experience, doctorResult.Experience);
            Assert.AreEqual(doctor.SpecializationID, doctorResult.SpecializationID);
            Assert.AreEqual(doctor.Qualification, doctorResult.Qualification);
            Assert.AreEqual(doctor.AdminId, doctorResult.AdminId);
        }



    }
}

