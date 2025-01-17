
namespace DataTransferObjects
{
    public class OrderViewDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public double TotalAmount { get; set; }
        public CartDto? CartDo { get; set; }
    }
}
