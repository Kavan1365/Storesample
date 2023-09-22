using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Events
{
    public class FactorCheckoutEvent
    {
        public long OrderId { get; set; }
        public int? StoreId { get; set; }
        public int TypeCost { get; set; }
        public int? UserId { get; set; }
        public bool IsPayByWallet { get; set; }
        public bool IsMoudule { get; set; }
        public bool IsWallet { get; set; }
        public string modules { get; set; }
    }

}
