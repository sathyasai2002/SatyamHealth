using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SatyamHealthCare.Controllers;
using SatyamHealthCare.Models;
using SatyamHealthCare.IRepos;
using System.Collections.Generic;
using System.Threading.Tasks;
using SatyamHealthCare.Repositories;

namespace SatyamHealthCareTests.Controllers
{
    [TestFixture]
    public class SpecializationsControllerTests
    {
        private SpecializationsController _controller;
        private Mock<ISpecialization> _mockSpecializationService;

        [SetUp]
        public void SetUp()
        {
            _mockSpecializationService = new Mock<ISpecialization>();
            _controller = new SpecializationsController(_mockSpecializationService.Object);
        }

        [Test]
        public async Task GetSpecializations_ReturnsOk_WhenSpecializationsExist()
        {
            // Arrange
            var specializations = new List<Specialization>
            {
                new Specialization { SpecializationID = 1, SpecializationName = "Cardiology" },
                new Specialization { SpecializationID = 2, SpecializationName = "Neurology" }
            };

            _mockSpecializationService
                .Setup(service => service.GetAllSpecializationsAsync())
                .ReturnsAsync(specializations);

            // Act
            var result = await _controller.GetSpecializations();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(specializations, okResult.Value);
        }

        [Test]
        public async Task GetSpecializations_ReturnsNotFound_WhenNoSpecializationsExist()
        {
            // Arrange
            _mockSpecializationService
                .Setup(service => service.GetAllSpecializationsAsync())
                .ReturnsAsync((IEnumerable<Specialization>)null);

            // Act
            var result = await _controller.GetSpecializations();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}
