using System;

namespace EventBusRabbitMQ.Events
{
  
    public class UserViewModelEventBus
    {
        public string FullName { get; set; }
        public bool Active { get; set; }
        public string UserName { get; set; }
    }
}
