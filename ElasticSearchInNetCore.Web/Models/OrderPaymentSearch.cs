using System;

namespace ElasticSearchInNetCore.Web
{
    public class OrderPaymentSearch
    {
        public int OrderPaymentHistoryId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal PaymentAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentStatus { get; set; }
    }
}