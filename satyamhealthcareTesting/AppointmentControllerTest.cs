using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.Controllers;
using Microsoft.AspNetCore.Http;
using SatyamHealthCare.DTO;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using SatyamHealthCare.Exceptions;
using static SatyamHealthCare.Constants.Status;
using System.Linq.Expressions;

namespace SatyamHealthCare.Tests.Controllers
{
    [TestFixture]
    public class AppointmentsControllerTests
    {
        private SatyamDbContext _context;
        private Mock<IAppointment> _mockAppointmentRepo;
        private Mock<INotificationService> _mockNotificationService;
        private AppointmentsController _controller;

        [SetUp]
        public void Setup()
        {
            // Create in-memory database options
            var options = new DbContextOptionsBuilder<SatyamDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Initialize the context and mocks
            _context = new SatyamDbContext(options);
            _mockAppointmentRepo = new Mock<IAppointment>();
            _mockNotificationService = new Mock<INotificationService>();

            // Create the controller instance
            _controller = new AppointmentsController(_context, _mockAppointmentRepo.Object, _mockNotificationService.Object);

            // Seed the database with test data
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Add test data for patients and doctors
            var patient = new Patient
            {
                PatientID = 1,
                Email = "patient@test.com",
                FullName = "John Doe",
                ContactNumber = "1234567890",
                Address = "123 Test St",
                BloodGroup = "O+",
                City = "Test City",
                Gender = "Male",
                Password = "Test@Password123", // Make sure this matches your password policy
                Pincode = "123456",
                State = "Test State"
            };

            var doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. Smith",
                Designation = "General Practitioner",
                Email = "drsmith@test.com",
                Password = "DrSmith@Password123",
                PhoneNo = "0987654321",
                Qualification = "MD"
            };

            _context.Patients.Add(patient);
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }


        [Test]
        public async Task PostAppointment_CreatesAppointmentSuccessfully()
        {
            // Arrange
            var appointmentDto = new AppointmentDTO
            {
                PatientId = 1,
                DoctorId = 1,
                AppointmentDate = DateTime.UtcNow.AddDays(1),
                AppointmentTime = new TimeSpan(11, 0, 0),
                Status = 0,
                Symptoms = "Fever"
            };

            var createdAppointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = appointmentDto.AppointmentDate.Value,
                AppointmentTime = appointmentDto.AppointmentTime
            };

            _mockAppointmentRepo.Setup(a => a.AddAppointment(It.IsAny<Appointment>())).ReturnsAsync(createdAppointment);

            // Act
            var result = await _controller.PostAppointment(appointmentDto);

            // Assert
            var actionResult = result as ActionResult<Appointment>;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<CreatedAtActionResult>(actionResult.Result); // Check if the result is CreatedAtActionResult

            var createdAtActionResult = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);

            var response = createdAtActionResult.Value as AppointmentResponseDTO;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.AppointmentId);
            Assert.AreEqual("Appointment is created successfully", response.Message);

            // Verify the notification was sent
            _mockNotificationService.Verify(n => n.SendAppointmentConfirmationAsync(
                "patient@test.com",
                "1234567890",
                "John Doe",
                "Dr. Smith",
                It.IsAny<DateTime>(),
                It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Test]
        public async Task GetAppointments_ReturnsUnauthorized_WhenEmailClaimMissing()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                // No email claim
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.GetAppointments();

            // Assert
            var actionResult = result.Result as UnauthorizedObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(401, actionResult.StatusCode);
            Assert.AreEqual("User email not found in the token.", actionResult.Value);
        }
        [Test]
        public async Task GetAppointments_ReturnsAppointments_ForAdminUser()
        {
            // Arrange
            var appointments = new List<Appointment>
    {
        new Appointment
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 1,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            AppointmentTime = new TimeSpan(11, 0, 0),
            Status = AppointmentStatus.Pending,
            Symptoms = "Fever",
            Doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. Smith",
                PhoneNo = "1234567890",
                Email = "drsmith@test.com",
                Password = "DrSmith@Password123",
                Designation = "General Practitioner",
                Experience = 10,
                Qualification = "MD"
            },
            Patient = new Patient
            {
                FullName = "John Doe"
            }
        }
    };

            _mockAppointmentRepo.Setup(a => a.GetAllAppointments()).ReturnsAsync(appointments);

            var userClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, "admin@test.com"),
        new Claim(ClaimTypes.Role, "Admin")
    };

            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.GetAppointments();

            // Assert
            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(200, actionResult.StatusCode);

            var returnedAppointments = actionResult.Value as List<AppointmentDTO>;
            Assert.IsNotNull(returnedAppointments);
            Assert.AreEqual(1, returnedAppointments.Count);
            Assert.AreEqual("John Doe", returnedAppointments[0].PatientName);
            Assert.AreEqual(1, returnedAppointments[0].DoctorId);
        }

        [Test]
        public async Task GetAppointmentById_ReturnsAppointment_WhenAppointmentExists()
        {
            // Arrange
            var appointmentId = 1;
            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = 1,
                DoctorId = 1,
                AppointmentDate = DateTime.UtcNow.AddDays(1),
                AppointmentTime = new TimeSpan(11, 0, 0),
                Status = AppointmentStatus.Pending,
                Symptoms = "Fever"
            };

            _mockAppointmentRepo.Setup(a => a.GetAppointmentById(appointmentId)).ReturnsAsync(appointment);

            // Act
            var result = await _controller.GetAppointment(appointmentId);

            // Assert
            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(200, actionResult.StatusCode);

            var returnedAppointment = actionResult.Value as AppointmentDTO;
            Assert.IsNotNull(returnedAppointment);
            Assert.AreEqual(appointmentId, returnedAppointment.AppointmentId);
            Assert.AreEqual("Fever", returnedAppointment.Symptoms);
        }

        [Test]
        public void GetAppointmentById_ThrowsAppointmentNotFoundException_WhenAppointmentDoesNotExist()
        {
            // Arrange
            var appointmentId = 1;

            // Mock the repository to throw the AppointmentNotFoundException when appointment is not found
            _mockAppointmentRepo.Setup(a => a.GetAppointmentById(appointmentId))
                .ThrowsAsync(new AppointmentNotFoundException($"Appointment with ID {appointmentId} not found."));

            // Act & Assert
            var exception = Assert.ThrowsAsync<AppointmentNotFoundException>(async () =>
                await _controller.GetAppointment(appointmentId));

            Assert.AreEqual($"Appointment with ID {appointmentId} not found.", exception.Message);
        }

        [Test]
        public async Task CancelAppointment_ReturnsOk_WhenCancellationIsSuccessful()
        {
            // Arrange
            var appointmentId = 1;

            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                Status = AppointmentStatus.Pending,
                PatientId = 1,
                DoctorId = 1,
                AppointmentDate = DateTime.UtcNow.AddDays(1),
                AppointmentTime = new TimeSpan(11, 0, 0),
                Symptoms = "Headache"
            };

            // Add the appointment to the in-memory database
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Act
            // Intentionally pass an invalid ID to simulate an error
            var result = await _controller.CancelAppointment(appointmentId + 99); // Invalid ID

            // Assert
            var actionResult = result as ObjectResult; // Change to ObjectResult to catch all scenarios
            Assert.IsNotNull(actionResult, "Expected actionResult to not be null.");

            // Check the status code for an error scenario
            Assert.AreEqual(500, actionResult.StatusCode, "Expected a 500 status code due to invalid appointment ID.");

            // Check the message in the response
            Assert.IsTrue(actionResult.Value is string, "Expected a string message.");

            var errorMessage = actionResult.Value as string; // Cast to string safely

            // Validate the message content
            Assert.AreEqual("Internal server error: Appointment with ID 100 not found.", errorMessage, "The error message does not match the expected message.");
        }

        [TearDown]
        public void TearDown()
        {

            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


    }
}
