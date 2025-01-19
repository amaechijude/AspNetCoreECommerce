using AspNetCoreEcommerce.Services.Contracts;
using AspNetCoreEcommerce.Respositories.Contracts;
using AspNetCoreEcommerce.Respositories.Implementations;

namespace AspNetCoreEcommerce.Services.Implementations
{
    public class CartItemService(ICartItemRepository cartItemRepository) : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
        public async Task ADddToCartAsync(int customerId, int productId)
        {
            try
            {
                var cartItem = await _cartItemRepository.ADddToCartAsync(customerId, productId);
            }
            catch (ProductNotFoundException)
            {
                throw new ProductNotFoundException();
            }
            catch (ItemAlreadyInCartException)
            {
                throw new ItemAlreadyInCartException();
            }
            catch (CustomerNotFoundException)
            {
                throw new CustomerNotFoundException();
            }
        }
        public async Task RemoveFromCartAsync(int customerId, int productId)
        {
            try
            {
                await _cartItemRepository.RemoveFromCartAsync(customerId, productId);
            }
            catch (ProductNotFoundException)
            {
                throw new ProductNotFoundException();
            }
            catch (ItemNotFoundException)
            {
                throw new ItemNotFoundException();
            }
            catch (CustomerNotFoundException)
            {
                throw new CustomerNotFoundException();
            }
        }
    }
}