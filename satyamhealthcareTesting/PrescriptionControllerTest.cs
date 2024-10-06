using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SatyamHealthCare.Controllers;
using SatyamHealthCare.DTOs;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.Tests.Controllers
{
    [TestFixture]
    public class PrescriptionsControllerTests
    {
        private PrescriptionsController _controller;
        private Mock<IPrescription> _mockPrescriptionService;

        [SetUp]
        public void SetUp()
        {
            _mockPrescriptionService = new Mock<IPrescription>();
            _controller = new PrescriptionsController(_mockPrescriptionService.Object);
        }

        [Test]
        public async Task GetAllPrescriptions_ReturnsOk_WithListOfPrescriptions()
        {
            // Arrange
            var prescriptions = new List<Prescription>
            {
                new Prescription
                {
                    PrescriptionID = 1,
                    NoOfDays = 7,
                    Dosage = "1 pill",
                    BeforeAfterFood = "Before",
                    Remark = "Take with water",
                    PrescriptionMedicines = new List<PrescriptionMedicine>
                    {
                        new PrescriptionMedicine { MedicineID = 1 },
                        new PrescriptionMedicine { MedicineID = 2 }
                    },
                    PrescriptionTests = new List<PrescriptionTest>
                    {
                        new PrescriptionTest { TestID = 1 }
                    }
                },
                new Prescription
                {
                    PrescriptionID = 2,
                    NoOfDays = 5,
                    Dosage = "2 pills",
                    BeforeAfterFood = "After",
                    Remark = "Take after meals",
                    PrescriptionMedicines = new List<PrescriptionMedicine>
                    {
                        new PrescriptionMedicine { MedicineID = 3 }
                    },
                    PrescriptionTests = new List<PrescriptionTest>
                    {
                        new PrescriptionTest { TestID = 2 }
                    }
                }
            };

            _mockPrescriptionService.Setup(service => service.GetAllPrescriptionsAsync()).ReturnsAsync(prescriptions);

            // Act
            var result = await _controller.GetAllPrescriptions();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedPrescriptions = okResult.Value as List<PrescriptionDTO>;
            Assert.AreEqual(2, returnedPrescriptions.Count);
        }

        [Test]
        public async Task GetPrescriptionById_ValidId_ReturnsOk_WithPrescription()
        {
            // Arrange
            var prescription = new Prescription
            {
                PrescriptionID = 1,
                NoOfDays = 7,
                Dosage = "1 pill",
                BeforeAfterFood = "Before",
                Remark = "Take with water",
                PrescriptionMedicines = new List<PrescriptionMedicine>
                {
                    new PrescriptionMedicine { MedicineID = 1 }
                },
                PrescriptionTests = new List<PrescriptionTest>
                {
                    new PrescriptionTest { TestID = 1 }
                }
            };

            _mockPrescriptionService.Setup(service => service.GetPrescriptionById(1)).ReturnsAsync(prescription);

            // Act
            var result = await _controller.GetPrescriptionById(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedPrescription = okResult.Value as PrescriptionDTO;
            Assert.AreEqual(prescription.PrescriptionID, returnedPrescription.PrescriptionID);
        }

        [Test]
        public async Task GetPrescriptionById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockPrescriptionService.Setup(service => service.GetPrescriptionById(99)).ReturnsAsync((Prescription)null);

            // Act
            var result = await _controller.GetPrescriptionById(99);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task AddPrescription_ValidPrescription_ReturnsCreated()
        {
            // Arrange
            var prescriptionDto = new PrescriptionDTO
            {
                NoOfDays = 7,
                Dosage = "1 pill",
                BeforeAfterFood = "Before",
                Remark = "Take with water",
                MedicineIDs = new List<int> { 1, 2 },
                TestIDs = new List<int> { 1 }
            };

            var prescription = new Prescription
            {
                // The PrescriptionID will usually be set by the database; 
                // assuming it's auto-generated after insertion.
                NoOfDays = prescriptionDto.NoOfDays,
                Dosage = prescriptionDto.Dosage,
                BeforeAfterFood = prescriptionDto.BeforeAfterFood,
                Remark = prescriptionDto.Remark,
                PrescriptionMedicines = prescriptionDto.MedicineIDs.Select(id => new PrescriptionMedicine { MedicineID = id }).ToList(),
                PrescriptionTests = prescriptionDto.TestIDs.Select(id => new PrescriptionTest { TestID = id }).ToList()
            };

            // Setup the mock service to return nothing for AddPrescriptionAsync (void return type).
            _mockPrescriptionService
                .Setup(service => service.AddPrescriptionAsync(It.IsAny<Prescription>()))
                .Callback<Prescription>(pres => pres.PrescriptionID = 1) // Simulating that the PrescriptionID is set after the "save".
                .Returns(Task.CompletedTask); // Since AddPrescriptionAsync returns void (Task)

            // Act
            var result = await _controller.AddPrescription(prescriptionDto);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetPrescriptionById", createdAtActionResult.ActionName);
            Assert.AreEqual(1, ((PrescriptionDTO)createdAtActionResult.Value).PrescriptionID); // Assert that the ID is set correctly
        }




    }
}
