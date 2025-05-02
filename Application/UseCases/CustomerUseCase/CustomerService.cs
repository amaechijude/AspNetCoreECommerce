using AspNetCoreEcommerce.Application.Interfaces.Repositories;
using AspNetCoreEcommerce.Application.Interfaces.Services;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Application.UseCases.CustomerUseCase
{
    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        public async Task<ResultPattern> CreateCustomerAsync(User user, string firstname, string lastName)
        {
            var userCustomer = await _customerRepository.GetCustomerByUserIdAsync(user.Id);
            if (userCustomer is not null)
                return ResultPattern.FailResult("");

            var customer = new Customer
            {
                CustomerID = Guid.CreateVersion7(),
                UserId = user.Id,
                User = user,
                FirstName = firstname,
                LastName = lastName,
                Email = user.Email
            };
            var createCustomer = await _customerRepository.CreateCustomerAsync(customer);
            user.Customer = createCustomer;
            user.CustomerID = createCustomer.CustomerID;
            await _customerRepository.SaveChangesAsync();
            if (createCustomer is null)
                return ResultPattern.FailResult("Failed to create customer");
            return ResultPattern.SuccessResult(createCustomer);
        }

        public Task<ResultPattern> DeleteCustomerAsync(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultPattern> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer is null)
                return ResultPattern.FailResult("Customer not found");
            return ResultPattern.SuccessResult(customer);
        }
    }
}