namespace AspNetCoreEcommerce.DTOs
{
    public class OrderViewDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public double TotalAmount { get; set; }
        public CartItemDto? CartDo { get; set; }
    }
}
