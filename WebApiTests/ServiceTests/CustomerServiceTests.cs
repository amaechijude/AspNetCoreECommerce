using Xunit;
using Moq;
using AspNetCoreEcommerce.Repositories.Contracts;
using AspNetCoreEcommerce.Authentication;
using System.Threading.Channels;
using AspNetCoreEcommerce.EmailService;
using AspNetCoreEcommerce.Services.Implementations;
using AspNetCoreEcommerce.DTOs;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.WebApiTests
{
    public class CustomerServiceTests
    {
       private readonly Mock<ICustomerRepository> _customerRepositoryMock;
       private readonly Mock<TokenProvider> _tokenProviderMock;
       private readonly Channel<EmailDto> _emailChannel;
       private readonly CustomerService _customerService;

       public CustomerServiceTests()
       {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _tokenProviderMock = new Mock<TokenProvider>();
            _emailChannel = Channel.CreateUnbounded<EmailDto>();

            _customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _tokenProviderMock.Object,
                _emailChannel
            );
       }

       [Fact]
       public async Task CreateCustomerTests()
       {
            var customerDto = new CustomerRegistrationDTO
            {
                CustomerEmail = "test@example.com",
                Password = "Test@123",
                FirstName = "John",
                LastName = "Doe",
                CustomerPhone = "1234567890"
            };
            var cid = Guid.NewGuid();

            _customerRepositoryMock.Setup(repo => repo.GetCustomerByEmailAsync(customerDto.CustomerEmail))
                    .ReturnsAsync((Customer?)null);

            _customerRepositoryMock.Setup(repo => repo.CreateCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync(new Customer{
                    CustomerEmail = customerDto.CustomerEmail
                });

            var result = await _customerService.CreateCustomerAsync(customerDto);

            Assert.True(result.Success);

       }
    }
}