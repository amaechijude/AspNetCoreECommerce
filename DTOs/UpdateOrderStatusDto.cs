using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreEcommerce.Entities;

namespace AspNetCoreEcommerce.DTOs
{
    public class UpdateOrderStatusDto
{
    public OrderStatusEnum OrderStatus { get; set; }
}

}