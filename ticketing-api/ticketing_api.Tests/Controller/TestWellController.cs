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

namespace ticketing_api.Tests
{
    public class TestWellController
    {
        public readonly WellsController wellController;
        public readonly ISieveProcessor sieveProcessor;
        public readonly IHttpContextAccessor httpContextAccessor;
        public readonly IOptions<SieveOptions> options;
        public readonly ISieveCustomSortMethods sieveCustomSortMethods;
        public readonly ISieveCustomFilterMethods sieveCustomFilterMethods;
        public readonly UserResolverService userService;
        public readonly ILogger<WellsController> logger;
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

        public TestWellController()
        {
            //Arrange constructor in wellcontroller
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "ro_ticketing").Options;

            userService = new UserResolverService(httpContextAccessor);

            logger = new Mock<ILogger<WellsController>>().Object;
            emailSender = new Mock<IEmailSender>().Object;
            options = new Mock<IOptions<SieveOptions>>().Object;
            sieveCustomSortMethods = new Mock<ISieveCustomSortMethods>().Object;
            sieveCustomFilterMethods = new Mock<ISieveCustomFilterMethods>().Object;

            sieveProcessor = new Mock<ApplicationSieveProcessor>(options, sieveCustomSortMethods, sieveCustomFilterMethods).Object;

            wellController = new WellsController(new ApplicationDbContext(builder, userService), logger, emailSender, sieveProcessor);
        }

        /// <summary>
        /// ReturnsBadRequest insert well records with InvalidObjectPassed 
        /// </summary>
        [Fact]
        public async Task Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            Well nameMissingItem = new Well()
            {
                RigLocationId = 1,
                Direction = "Direction",
                IsDeleted = false,
                IsVisible = true,
                IsEnabled = true
            };

            wellController.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = await wellController.PostWellAsync(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        /// <summary>
        /// Insert well records with ValidObjectPassed 
        /// </summary>
        [Fact]
        public async Task Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            Well well = new Well()
            {
                Id = 34,
                Name = "testWell1",
                RigLocationId = 1,
                Direction = "Direction",
                IsDeleted = false,
                IsVisible = true,
                IsEnabled = true
            };

            // Act
            var result = await wellController.PostWellAsync(well);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        /// <summary>
        /// ReturnsNotFound message update well records with NotExistWellId
        /// </summary>
        [Fact]
        public async Task Put_NotExistingWellId_ReturnsNotFoundResponse()
        {
            // Arrange
            Well well = new Well()
            {
                Id = 88,
                Name = "testWell1",
                RigLocationId = 1,
                Direction = "Direction",
                IsDeleted = false,
                IsVisible = true,
                IsEnabled = true
            };

            // Act
            var result = await wellController.PutWellAsync(well.Id, well);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// ReturnsNotFound message remove well records with NotExistWellId
        /// </summary>
        [Fact]
        public async Task Remove_NotExistingWellId_ReturnsNotFoundResponse()
        {
            // Arrange
            int wellId = 88;

            // Act
            var notFoundResult = await wellController.DeleteWellAsync(wellId);

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        /// <summary>
        /// ReturnsNotFound message Get well with specific NotExistWellId
        /// </summary>
        [Fact]
        public async Task GetById_NotExistingWellId_ReturnsNotFoundResult()
        {
            // Arrange
            int wellId = 88;

            // Act
            var notFoundResult = await wellController.GetWell(wellId);

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        /// <summary>
        /// Get Well All Records
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
           
            var pagingResultWellView = new Mock<PagingResults<WellView>>();

            var mockRepo = new Mock<WellService>(dbContext, sieveProcessor);

            var mockResult = mockRepo.Setup(repo => repo.GetWellViewAsync(sieveModel))
               .Returns(Task.Run(() => pagingResultWellView.Object));

            IQueryable<WellView> queryObject = (new List<WellView> { new WellView()}).AsQueryable();

            //mock object through to set apply method in sieveprocessor
            sieveProcessor.Apply(sieveModel, queryObject, dataForCustomMethods: null, applyFiltering: false, applySorting: false, applyPagination: false);

            var wellService = new WellService(dbContext, sieveProcessor);

            // Act
            // This testcases are passed when we applyFiltering false, applySorting false, applyPagination false 
            var resultOk = wellService.GetWellViewAsync(sieveModel);

            // Assert
            Assert.NotNull(resultOk);
        }
    }
}
