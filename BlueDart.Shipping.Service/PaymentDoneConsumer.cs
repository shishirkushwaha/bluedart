using System;
using System.Threading.Tasks;
using BlueDart.Messaging.Events;
using MassTransit;

namespace BlueDart.Notification.Service
{
    public class PaymentDoneConsumer : IConsumer<IPaymentDoneEvent>
    {
        public async Task Consume(ConsumeContext<IPaymentDoneEvent> context)
        {
            //Start Shipping of the Product.
            await Console.Out.WriteLineAsync($"Order Shipped: Order id {context.Message.CorrelationId}");

            //Inform Saga for Status Update
            await context.Publish<IOrderShippedEvent>(
                new { CorrelationId = context.Message.CorrelationId });
        }
    }
}
