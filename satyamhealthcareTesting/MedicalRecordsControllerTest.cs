using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SatyamHealthCare.Controllers;
using SatyamHealthCare.DTO;
using SatyamHealthCare.Exceptions;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.Tests.Controllers
{
    [TestFixture]
    public class MedicalRecordsControllerTests
    {
        private Mock<IMedicalRecord> _mockMedicalRecordRepo;
        private MedicalRecordsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockMedicalRecordRepo = new Mock<IMedicalRecord>();
            _controller = new MedicalRecordsController(null, _mockMedicalRecordRepo.Object);
        }

        [Test]
        public async Task GetMedicalRecords_ReturnsOk_WithMedicalRecords()
        {
            // Arrange
            var medicalRecords = new List<MedicalRecord>
            {
                new MedicalRecord { RecordID = 1, PatientID = 1, DoctorID = 1, Diagnosis = "Flu", PrescriptionID = 1, MedicalHistoryId = 1 },
                new MedicalRecord { RecordID = 2, PatientID = 2, DoctorID = 2, Diagnosis = "Cold", PrescriptionID = 2, MedicalHistoryId = 2 }
            };

            _mockMedicalRecordRepo.Setup(repo => repo.GetAllMedicalRecordsAsync()).ReturnsAsync(medicalRecords);

            // Act
            var result = await _controller.GetMedicalRecords();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var records = okResult.Value as List<MedicalRecordDTO>;
            Assert.AreEqual(2, records.Count);
        }

        [Test]
        public async Task GetMedicalRecords_NoRecords_ThrowsException()
        {
            // Arrange
            _mockMedicalRecordRepo.Setup(repo => repo.GetAllMedicalRecordsAsync()).ReturnsAsync(new List<MedicalRecord>());

            // Act & Assert
            Assert.ThrowsAsync<MedicalRecordNotFoundException>(async () => await _controller.GetMedicalRecords());
        }

        [Test]
        public async Task GetMedicalRecord_InvalidId_ThrowsException()
        {
            // Arrange
            _mockMedicalRecordRepo.Setup(repo => repo.GetMedicalRecordByIdAsync(1)).ReturnsAsync((MedicalRecord)null);

            // Act & Assert
            Assert.ThrowsAsync<MedicalRecordNotFoundException>(async () => await _controller.GetMedicalRecord(1));
        }

        [Test]
        public async Task PutMedicalRecord_ValidId_UpdatesRecord()
        {
            // Arrange
            var medicalRecordDto = new MedicalRecordDTO { RecordID = 1, PatientID = 1, DoctorID = 1, Diagnosis = "Flu", PrescriptionID = 1, MedicalHistoryId = 1 };
            _mockMedicalRecordRepo.Setup(repo => repo.UpdateMedicalRecordAsync(It.IsAny<MedicalRecord>())).Returns(Task.CompletedTask);
            _mockMedicalRecordRepo.Setup(repo => repo.GetMedicalRecordByIdAsync(1)).ReturnsAsync(new MedicalRecord());

            // Act
            var result = await _controller.PutMedicalRecord(1, medicalRecordDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            _mockMedicalRecordRepo.Verify(repo => repo.UpdateMedicalRecordAsync(It.IsAny<MedicalRecord>()), Times.Once);
        }

        [Test]
        public async Task PutMedicalRecord_InvalidId_ThrowsException()
        {
            // Arrange
            var medicalRecordDto = new MedicalRecordDTO { RecordID = 2, PatientID = 1, DoctorID = 1, Diagnosis = "Flu", PrescriptionID = 1, MedicalHistoryId = 1 };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PutMedicalRecord(1, medicalRecordDto));
        }

        [Test]
        public async Task DeleteMedicalRecord_ValidId_DeletesRecord()
        {
            // Arrange
            _mockMedicalRecordRepo.Setup(repo => repo.GetMedicalRecordByIdAsync(1)).ReturnsAsync(new MedicalRecord());
            _mockMedicalRecordRepo.Setup(repo => repo.DeleteMedicalRecordAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteMedicalRecord(1);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            _mockMedicalRecordRepo.Verify(repo => repo.DeleteMedicalRecordAsync(1), Times.Once);
        }

        [Test]
        public async Task DeleteMedicalRecord_InvalidId_ThrowsException()
        {
            // Arrange
            _mockMedicalRecordRepo.Setup(repo => repo.GetMedicalRecordByIdAsync(1)).ReturnsAsync((MedicalRecord)null);

            // Act & Assert
            Assert.ThrowsAsync<MedicalRecordNotFoundException>(async () => await _controller.DeleteMedicalRecord(1));
        }
    }
}
