using System;
using System.Threading.Tasks;
using BlueDart.Messaging.Events;
using MassTransit;

namespace BlueDart.Payment.Service
{
    public class PaymentRequiredConsumer : IConsumer<IPaymentRequiredEvent>
    {
        public static int Count = 0;

        public async Task Consume(ConsumeContext<IPaymentRequiredEvent> context)
        {
            Random r = new Random();
            int randomInt = r.Next(1, 10);

            //Payment Failed when Counter is an Odd Number.
            if (randomInt < 5)
            {
                await Console.Out.WriteLineAsync($"Payment Problem. Server Down: Order id {context.Message.CorrelationId}");
                throw new Exception("Payment Problem. Server Down");
            }

            //Take Payment from Users Bank
            await Console.Out.WriteLineAsync($"Payment Recieved from {context.Message.PickupName} for: Order id {context.Message.CorrelationId}");

            await context.Publish<IPaymentDoneEvent>(
                new { CorrelationId = context.Message.CorrelationId });
        }
    }
}
