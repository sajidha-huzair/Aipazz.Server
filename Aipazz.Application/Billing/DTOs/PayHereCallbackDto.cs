using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.DTOs
{
    public class PayHereCallbackDto
    {
        public string merchant_id { get; set; }=string.Empty;
        public string order_id { get; set; } = string.Empty;
        public string payment_id { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;// "2" means paid
        public string md5sig { get; set; } = string.Empty; // hash to verify
        public string paid_amount { get; set; } = string.Empty;
        public string custom_1 { get; set; }= string.Empty;
        public string currency { get; set; } = string.Empty;
        public string method { get; set; } = string.Empty;
    }

}
