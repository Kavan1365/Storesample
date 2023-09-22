using EventBusRabbitMQ.Events;

namespace EventBusRabbitMQ.Producer
{
    public interface IEventBusRabbitMQProducer
    {
        void PublishFileCheckout(string queueName, FileCheckoutEvent publishModel);
    }
}
