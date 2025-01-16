using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CartItem
    {
        public int CartId { get; set; }
        public ICollection<Product>? Products { get; set; }
        public double TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

    }
}
