using AspNetCoreEcommerce.Domain.Enums;

namespace AspNetCoreEcommerce.Application.UseCases.OrderUseCase
{
    public class UpdateOrderStatusDto
{
    public OrderStatusEnum OrderStatus { get; set; }
}

}