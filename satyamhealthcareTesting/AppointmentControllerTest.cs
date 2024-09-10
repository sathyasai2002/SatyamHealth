using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.Controllers;
using SatyamHealthCare.DTO;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using static SatyamHealthCare.Constants.Enum;

namespace SatyamHealthCare.Tests.Controllers
{
    [TestFixture]
    public class AppointmentsControllerTests
    {
        private Mock<IAppointment> _mockAppointmentRepo;
        private AppointmentsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockAppointmentRepo = new Mock<IAppointment>();
            _controller = new AppointmentsController(null, _mockAppointmentRepo.Object);
        }

        [Test]
        public async Task GetAppointments_ReturnsOkResult_WithListOfAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment { AppointmentId = 1, PatientId = 1, DoctorId = 1, Status = AppointmentStatus.Pending, AppointmentDate = System.DateTime.Now },
                new Appointment { AppointmentId = 2, PatientId = 2, DoctorId = 2, Status = AppointmentStatus.Confirmed, AppointmentDate = System.DateTime.Now }
            };

            _mockAppointmentRepo.Setup(repo => repo.GetAllAppointments()).ReturnsAsync(appointments);

            // Act
            var result = await _controller.GetAppointments();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var resultAppointments = okResult.Value as IEnumerable<AppointmentDTO>;

            Assert.AreEqual(appointments.Count, resultAppointments.Count());
        }

        [Test]
        public async Task GetAppointment_ReturnsOk_WhenAppointmentExists()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                Status = AppointmentStatus.Pending,
                AppointmentDate = System.DateTime.Now
            };

            _mockAppointmentRepo.Setup(repo => repo.GetAppointmentById(1)).ReturnsAsync(appointment);

            // Act
            var result = await _controller.GetAppointment(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAppointment_ReturnsNotFound_WhenAppointmentDoesNotExist()
        {
            // Arrange
            _mockAppointmentRepo.Setup(repo => repo.GetAppointmentById(1)).ReturnsAsync((Appointment)null);

            // Act
            var result = await _controller.GetAppointment(1);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task PostAppointment_ReturnsCreatedAtAction_WhenAppointmentIsCreated()
        {
            // Arrange
            var appointmentDto = new AppointmentDTO
            {
                PatientId = 1,
                DoctorId = 1,
                Status = AppointmentStatus.Pending,
                AppointmentDate = System.DateTime.Now
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                Status = AppointmentStatus.Pending,
                AppointmentDate = System.DateTime.Now
            };

            _mockAppointmentRepo.Setup(repo => repo.AddAppointment(It.IsAny<Appointment>())).ReturnsAsync(appointment);

            // Act
            var result = await _controller.PostAppointment(appointmentDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        }

        [Test]
        public async Task UpdateAppointment_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateDto = new UpdateAppointmentDTO
            {
                AppointmentId = 1,
              //  PatientId = 1,
                DoctorId = 1,
                AppointmentDate = System.DateTime.Now,
                Status = AppointmentStatus.Confirmed
            };

            _mockAppointmentRepo.Setup(repo => repo.UpdateAppointment(1, updateDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateAppointment(1, updateDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteAppointment_ReturnsNoContent_WhenAppointmentIsDeleted()
        {
            // Arrange
            _mockAppointmentRepo.Setup(repo => repo.DeleteAppointment(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAppointment(1);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteAppointment_ReturnsNotFound_WhenAppointmentDoesNotExist()
        {
            // Arrange
            _mockAppointmentRepo.Setup(repo => repo.DeleteAppointment(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteAppointment(1);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
