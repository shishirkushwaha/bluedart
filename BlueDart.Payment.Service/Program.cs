using System;
using BlueDart.Messaging;
using MassTransit;
using GreenPipes;

namespace BlueDart.Payment.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Payment";

            var bus = BusConfigurator.ConfigureBus();

            var handle = bus.ConnectReceiveEndpoint(RabbitMqConstants.PaymentServiceQueue, x =>
            {
                x.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(2)));
                x.Consumer<PaymentRequiredConsumer>();
            });

            bus.Start();

            Console.WriteLine("Listening for Payment Required events.. Press enter to exit");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
