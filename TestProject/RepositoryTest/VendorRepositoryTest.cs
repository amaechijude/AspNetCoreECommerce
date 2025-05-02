using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.RepositoryTest
{
    class VendorRepositoryTest
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var vendorRepository = new VendorRepository();
            // Act
            var vendors = vendorRepository.GetAllVendors();
            // Assert
            Assert.NotNull(vendors);
            Assert.IsType<List<Vendor>>(vendors);
            Assert.True(vendors.Count > 0);
        }
    }
}
