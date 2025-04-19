using AspNetCoreEcommerce.Data;
using AspNetCoreEcommerce.Entities;
using AspNetCoreEcommerce.Repositories.Implementations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AspNetCoreEcommerce.WebApiTests.RepositoryTests
{
    public class VendorRepositoryTests : IDisposable
    {
        private readonly string _dbPath = GlobalConstants._dbPath;
        private readonly SqliteConnection _sqliteConnection;
        private readonly ApplicationDbContext _context;
        private readonly VendorRepository _vendorRepository;

        public VendorRepositoryTests()
        {
            GlobalConstants.TestDbCheck();
            _sqliteConnection = new SqliteConnection($"DataSource={_dbPath}");
            _sqliteConnection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_sqliteConnection)
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            _vendorRepository = new VendorRepository(_context);
        }

        [Fact]
        public async Task SignupVendorAsyncTest()
        {
            // Arrange
            var vendor = new Vendor
            {
                VendorId = Guid.CreateVersion7(),
                VendorName = "TestVendoritOk",
                VendorEmail = "testg@example.com",
                PasswordHash = "password",
                Location = "TestLocation"
            };

            // Act
            var createdVendor = await _vendorRepository.SignupVendorAsync(vendor);
            var vendorEmailExists = await _vendorRepository.CheckExistingVendorEmailAsync(vendor.VendorEmail);
            var vendorNameExists = await _vendorRepository.CheckExistingVendorNameAsync(vendor.VendorName);
            
            // Assert
            Assert.NotNull(createdVendor);
            Assert.Equal(vendor.VendorId, createdVendor.VendorId);
            Assert.True(vendorEmailExists);
        }

        public void Dispose()
        {
            _context.Dispose();
            _sqliteConnection.Close();
            _sqliteConnection.Dispose();
        }
    }
}
