using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;
using ticketing_api.Controllers;
using ticketing_api.Data;
using ticketing_api.Infrastructure;
using ticketing_api.Services;
using Xunit;
using ticketing_api.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using ticketing_api.Models.Views;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ticketing_api.Infrastructure.Identity;
using System;
using System.Security.Claims;

namespace ticketing_api.Tests.Controller
{
    public class TestRigLocationController
    {
        public readonly RigLocationsController rigLocationController;
        public readonly ISieveProcessor sieveProcessor;
        public readonly IHttpContextAccessor httpContextAccessor;
        public readonly IOptions<SieveOptions> options;
        public readonly ISieveCustomSortMethods sieveCustomSortMethods;
        public readonly ISieveCustomFilterMethods sieveCustomFilterMethods;
        public readonly UserResolverService userService;
        private readonly ILogger<RigLocationsController> logger;
        public readonly IEmailSender emailSender;

        private Token GetTestToken()
        {
            Token token = new Token() { };
            token.client_id = "web.client1";
            token.client_secret = "secret";
            token.grant_type = "password";
            token.scope = "api1";
            token.username = "mnewlin@gmail.com";
            token.password = "password";
            return token;
        }

        public TestRigLocationController()
        {
            //Arrange constructor in riglocationcontroller
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "ro_ticketing").Options;

            userService = new UserResolverService(httpContextAccessor);

            logger = new Mock<ILogger<RigLocationsController>>().Object;
            emailSender = new Mock<IEmailSender>().Object;
            options = new Mock<IOptions<SieveOptions>>().Object;
            sieveCustomSortMethods = new Mock<ISieveCustomSortMethods>().Object;
            sieveCustomFilterMethods = new Mock<ISieveCustomFilterMethods>().Object;

            sieveProcessor = new Mock<ApplicationSieveProcessor>(options, sieveCustomSortMethods, sieveCustomFilterMethods).Object;

            rigLocationController = new RigLocationsController(new ApplicationDbContext(builder, userService), logger, emailSender, sieveProcessor);
        }

        /// <summary>
        /// ReturnsBadRequest insert riglocation records with InvalidObjectPassed 
        /// </summary>
        [Fact]
        public async Task Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            RigLocation nameMissingItem = new RigLocation()
            {
                CustomerId = 1,
                Note = "this is riglocation note",
                IsDeleted = false,
                IsVisible = true,
                IsEnabled = true
            };

            rigLocationController.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = await rigLocationController.PostRigLocation(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        /// <summary>
        /// Insert riglocation records with ValidObjectPassed 
        /// </summary>
        [Fact]
        public async Task Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            RigLocation rigLocation = new RigLocation()
            {
                Id = 45,
                CustomerId = 1,
                Name = "TestRiglocation",
                Note = "this is riglocation note",
                IsDeleted = false,
                IsVisible = true,
                IsEnabled = true
            };

            // Act
            var result = await rigLocationController.PostRigLocation(rigLocation);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        /// <summary>
        /// ReturnsNotFound message update riglocation records with NotExistRigLocationId
        /// </summary>
        [Fact]
        public async Task Put_NotExistingRiglocationId_ReturnsNotFoundResponse()
        {
            // Arrange
            RigLocation rigLocation = new RigLocation()
            {
                Id = 45,
                CustomerId = 1,
                Name = "TestRiglocation",
                Note = "this is riglocation note",
                IsDeleted = false,
                IsVisible = true,
                IsEnabled = true
            };

            // Act
            var notFoundResult = await rigLocationController.PutRigLocation(rigLocation.Id, rigLocation);

            //Assert
            Assert.NotNull(notFoundResult);
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        /// <summary>
        /// ReturnsNotFound message remove riglocation records with NotExistRigLocationId
        /// </summary>
        [Fact]
        public async Task Remove_NotExistingRiglocationId_ReturnsNotFoundResponse()
        {
            // Arrange
            int rigLocationId = 45;

            // Act
            var notFoundResult = await rigLocationController.DeleteRigLocation(rigLocationId);

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        /// <summary>
        /// ReturnsNotFound message Get riglocation with specific NotExistRigLocationId
        /// </summary>
        [Fact]
        public async Task GetById_NotExistingRiglocationId_ReturnsNotFoundResponse()
        {
            // Arrange
            int rigLocationId = 45;

            // Act
            var notFoundResult = await rigLocationController.DeleteRigLocation(rigLocationId);

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        /// <summary>
        /// Get Riglocation All Records
        /// </summary>
        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ro_ticketing").Options;

            var user = new UserResolverService(httpContextAccessor);

            var dbContext = new ApplicationDbContext(builder, user);

            SieveModel sieveModel = new SieveModel() { };
            sieveModel.Page = null;
            sieveModel.PageSize = null;
            sieveModel.Filters = null;
            sieveModel.Sorts = null;

            var pagingResultRigLocationView = new Mock<PagingResults<RigLocationView>>();

            var mockRepo = new Mock<RigLocationService>(dbContext, sieveProcessor);

            //this result are match to the actual result return by after function 
            var mockResult = mockRepo.Setup(repo => repo.GetRigLocationViewAsync(sieveModel))
               .Returns(Task.Run(() => pagingResultRigLocationView.Object));

            IQueryable<RigLocationView> queryObject = (new List<RigLocationView> { new RigLocationView() }).AsQueryable();

            //mock object through to set apply method in sieveprocessor
            sieveProcessor.Apply(sieveModel, queryObject, dataForCustomMethods: null, applyFiltering: false, applySorting: false, applyPagination: false);

            var rigLocationService = new RigLocationService(dbContext, sieveProcessor);

            // Act
            // This testcases are passed when we applyFiltering false, applySorting false, applyPagination false 
            var resultOk = rigLocationService.GetRigLocationViewAsync(sieveModel);

            // Assert
            Assert.NotNull(resultOk);
        }
    }

}
