namespace AspNetCoreEcommerce.Services.Contracts
{
    public interface ICartItemService
    {
        public Task ADddToCartAsync(int customerId, int productId);
        public Task RemoveFromCartAsync(int customerId, int productId);
    }
}
