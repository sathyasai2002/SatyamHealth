using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.Controllers;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace SatyamHealthCare.Tests.Controllers
{
    public class PatientControllerTest
    {
        [TestFixture]
        public class PatientsControllerTests
        {
            private Mock<IPatient> _mockPatientRepo;
            private PatientsController _controller;

            [SetUp]
            public void SetUp()
            {
                _mockPatientRepo = new Mock<IPatient>();
                _controller = new PatientsController(null, _mockPatientRepo.Object);
            }

            // Unit Test for GetPatients
            [Test]
            public async Task GetPatients_ReturnsOkResult_WithListOfPatients()
            {
                // Arrange
                var patients = new List<Patient>
    {
        new Patient { PatientID = 1, FullName = "Patient1" },
        new Patient { PatientID = 2, FullName = "Patient2" }
    };
                _mockPatientRepo.Setup(repo => repo.GetAllPatients()).ReturnsAsync(patients);

                // Act
                var result = await _controller.GetPatients();

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                var okResult = result.Result as OkObjectResult;
                var resultPatients = okResult.Value as IEnumerable<Patient>;

                Assert.AreEqual(patients.Count, resultPatients?.Count());
            }

            [Test]
            public async Task PostPatient_ReturnsOk_WhenPatientIsCreated()
            {
                // Arrange
                var patientDto = new PatientDTO
                {
                    FullName = "Patient1",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = "M",
                    BloodGroup = "O+",
                    ContactNumber = "1234567890",
                    Email = "patient1@example.com",
                    Address = "123 Main St",
                    Pincode = "123456",
                    City = "Metropolis",
                    Password = "Password123",
                    //ProfilePicture = null
                };

                var patient = new Patient
                {
                    PatientID = 1,
                    FullName = "Patient1",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = "M",
                    BloodGroup = "O+",
                    ContactNumber = "1234567890",
                    Email = "patient1@example.com",
                    Address = "123 Main St",
                    Pincode = "123456",
                    City = "Metropolis",
                    Password = "Password123",
                    //ProfilePicture = null
                };

                _mockPatientRepo.Setup(repo => repo.AddPatient(It.IsAny<Patient>())).ReturnsAsync(patient);

                // Act
                var result = await _controller.PostPatient(patientDto);

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                var okResult = result.Result as OkObjectResult;

                Assert.IsNotNull(okResult, "OkObjectResult is null");

                var response = JObject.FromObject(okResult.Value);
                Assert.AreEqual("Patient registered successfully", response["message"].ToString());

                var patientResult = response["patient"].ToObject<Patient>();
                Assert.IsNotNull(patientResult, "Patient object is null");

                Assert.AreEqual(patient.PatientID, patientResult.PatientID);
                Assert.AreEqual(patient.FullName, patientResult.FullName);
                Assert.AreEqual(patient.DateOfBirth, patientResult.DateOfBirth);
                Assert.AreEqual(patient.Gender, patientResult.Gender);
                Assert.AreEqual(patient.BloodGroup, patientResult.BloodGroup);
                Assert.AreEqual(patient.ContactNumber, patientResult.ContactNumber);
                Assert.AreEqual(patient.Email, patientResult.Email);
                Assert.AreEqual(patient.Address, patientResult.Address);
                Assert.AreEqual(patient.Pincode, patientResult.Pincode);
                Assert.AreEqual(patient.City, patientResult.City);
                Assert.AreEqual(patient.Password, patientResult.Password);
                // Assert.AreEqual(patient.ProfilePicture, patientResult.ProfilePicture);
            }




            // Unit Test for DeletePatient
            [Test]
            public async Task DeletePatient_ReturnsNoContent_WhenPatientDeleted()
            {
                // Arrange
                var patientId = 1;
                var patient = new Patient { PatientID = patientId, FullName = "Patient1" };
                _mockPatientRepo.Setup(repo => repo.GetPatientById(patientId)).ReturnsAsync(patient);
                _mockPatientRepo.Setup(repo => repo.DeletePatient(patientId)).ReturnsAsync(true);

                // Act
                var result = await _controller.DeletePatient(patientId);

                // Assert
                Assert.IsInstanceOf<NoContentResult>(result);
            }

            // Unit Test for PutPatient (Update)
            [Test]
            public async Task PutPatient_ReturnsNoContent_WhenPatientIsUpdated()
            {
                // Arrange
                var patient = new Patient { PatientID = 1, FullName = "UpdatedPatient" };
                _mockPatientRepo.Setup(repo => repo.GetPatientById(1)).ReturnsAsync(patient);
                _mockPatientRepo.Setup(repo => repo.UpdatePatient(patient)).ReturnsAsync(patient);

                // Act
                //  var result = await _controller.PutPatient(1, patient);

                // Assert
                //  Assert.IsInstanceOf<NoContentResult>(result);
            }
        }
    }
}
