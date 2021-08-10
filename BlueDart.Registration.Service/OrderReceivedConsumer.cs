using System;
using System.Threading.Tasks;
using BlueDart.Messaging.Events;
using MassTransit;

namespace BlueDart.Registration.Service
{
    public class OrderReceivedConsumer : IConsumer<IOrderReceivedEvent>
    {
        public async Task Consume(ConsumeContext<IOrderReceivedEvent> context)
        {
            await Console.Out.WriteLineAsync($"Order for customer {context.Message.PickupName} registered");

            //publish event
            await context.Publish<IOrderRegisteredEvent>(
                new { CorrelationId = context.Message.CorrelationId });
        }
    }
}
