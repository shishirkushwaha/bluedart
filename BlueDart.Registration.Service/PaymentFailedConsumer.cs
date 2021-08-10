using System;
using System.Threading.Tasks;
using BlueDart.Messaging.Events;
using MassTransit;

namespace BlueDart.Registration.Service
{
    public class PaymentFailedConsumer : IConsumer<IPaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<IPaymentFailedEvent> context)
        {
            await Console.Out.WriteLineAsync($"Order for customer {context.Message.PickupName} cancelled, as the Payment Failed.");

            //publish event
            await context.Publish<IOrderRegisteredEvent>(
                new { CorrelationId = context.Message.CorrelationId });
        }
    }
}
