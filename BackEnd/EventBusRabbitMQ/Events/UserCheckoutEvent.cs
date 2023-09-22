using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Events
{
    public class UserCheckoutEvent
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string userfullname { get; set; }
    }

}
