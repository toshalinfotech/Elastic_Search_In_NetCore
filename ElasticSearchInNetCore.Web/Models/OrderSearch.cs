using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchInNetCore.Web
{
    public class OrderSearch
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal OrderAmount { get; set; }

        public string OrderStatus { get; set; }

        public List<OrderDetailSearch> OrderDetails { get; set; }

        public List<OrderPaymentSearch> OrderPayments { get; set; }

    }
}
