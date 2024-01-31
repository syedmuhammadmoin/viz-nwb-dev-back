using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApplicationTests.Repo
{
    [TestClass]
    public class RequisitionRepositoryTests
    {
        private ApplicationDbContext _dbContext;
        private IRequisitionRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory) 
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // Set up and configure your test database
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var httpContextAccessor = new HttpContextAccessor();

            _dbContext = new ApplicationDbContext(dbContextOptions, httpContextAccessor);

            // Ensure the database is created and migrated
            _dbContext.Database.EnsureCreated();

            _repository = new RequisitionRepository(_dbContext);
        }


        [TestMethod]
        public void RequisitionRepository2_Should_Return_Status_Counts()
        {
            // Act
            var result = _repository.SummarizedbyStatus();

            // Assert
            Assert.IsNotNull(result);
            // Add more assertions based on your specific test scenario
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Clean up resources after each test
            _dbContext.Dispose();
        }
    }
    [TestClass]
    public class HelloWorldTests
    {
        [TestMethod]
        public void ShouldReturnHelloWorld()
        {
            // Arrange
            var expectedMessage = "Hello, World!";

            // Act
            var actualMessage = GetMessage();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        private string GetMessage()
        {
            return "Hello, World!";
        }
    }
}
