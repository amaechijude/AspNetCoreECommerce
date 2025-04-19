using Xunit;
using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Repositories.Implementations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.WebApiTests.RepositoryTests
{
    public class CustomerRepositoryTests : IDisposable
    {
        private readonly string _dbPath = GlobalConstants._dbPath;
        private readonly SqliteConnection _sqliteConnection;
        private readonly ApplicationDbContext _context;
        private readonly CustomerRepository _customerRepository;

        public CustomerRepositoryTests()
        {
            GlobalConstants.TestDbCheck();
            _sqliteConnection = new SqliteConnection($"DataSource={_dbPath}");
            _sqliteConnection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_sqliteConnection)
                .Options;

            _context = new ApplicationDbContext(options);
            // _context.Database.EnsureCreated();
            _customerRepository = new CustomerRepository(_context);
        }
        [Fact]
        public async Task GetCustomerByIDAsyncTest()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerID = Guid.CreateVersion7(),
                CustomerEmail = "testg@example.com",
                FirstName = "John",
                LastName = "Doe",
                PasswordHash = "password"
            };
            // Act
            var createdCustomer = await _customerRepository.CreateCustomerAsync(customer);

            // Assert
            Assert.NotNull(createdCustomer);
            Assert.NotNull(createdCustomer.Cart);
            Assert.Equal(customer.CustomerID, createdCustomer.CustomerID);
        }

        public void Dispose()
        {
            _context.Dispose();
            _sqliteConnection.Close();
            _sqliteConnection.Dispose();

        }
    }
}
