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
    public class AdminsControllerTests
    {
        private Mock<IAdmin> _mockAdminRepo;
        private AdminsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockAdminRepo = new Mock<IAdmin>();
            _controller = new AdminsController(null, _mockAdminRepo.Object);
        }

        // Unit Test for GetAdmins
        [Test]
        public async Task GetAdmins_ReturnsOkResult_WithListOfAdmins()
        {
            // Arrange
            var admins = new List<Admin>
            {
                new Admin { AdminId = 1 },
                new Admin { AdminId = 2 }
            };
            _mockAdminRepo.Setup(repo => repo.GetAllAdmins()).ReturnsAsync(admins);

            // Act
            var result = await _controller.GetAdmins();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var resultAdmins = okResult.Value as IEnumerable<Admin>;

            Assert.AreEqual(admins.Count, resultAdmins?.Count());
        }

        [Test]
        public async Task GetAdminById_ReturnsOk_WhenAdminExists()
        {
            // Arrange
            var admin = new Admin { AdminId = 1, FullName = "Admin1", Email = "admin1@example.com", Password = "Password123" };
            _mockAdminRepo.Setup(repo => repo.GetAdminById(1)).ReturnsAsync(admin);

            // Act
            var result = await _controller.GetAdminById(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(admin, okResult?.Value);
        }
        [Test]
        public async Task PostAdmin_ReturnsCreatedAtAction_WhenAdminIsCreated()
        {
            // Arrange
            var admin = new Admin { AdminId = 1, FullName = "Admin1", Email = "admin1@example.com", Password = "Password123" };
            _mockAdminRepo.Setup(repo => repo.AddAdmin(It.IsAny<Admin>())).Returns(Task.CompletedTask);
            _mockAdminRepo.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostAdmin(admin);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.AreEqual(nameof(_controller.GetAdminById), createdAtActionResult.ActionName);
            Assert.AreEqual(admin.AdminId, ((Admin)createdAtActionResult.Value).AdminId);
        }

    }


}
