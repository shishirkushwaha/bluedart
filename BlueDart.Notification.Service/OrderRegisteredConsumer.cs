using System;
using System.Threading.Tasks;
using BlueDart.Messaging.Events;
using MassTransit;

namespace BlueDart.Notification.Service
{
    public class OrderRegisteredConsumer : IConsumer<IOrderRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<IOrderRegisteredEvent> context)
        {
            //Send notification to user via SMS | Push Notification
            await Console.Out.WriteLineAsync($"Customer notification sent: Order id {context.Message.CorrelationId}");
        }
    }
}
