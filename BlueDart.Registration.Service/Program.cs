using System;
using BlueDart.Messaging;
using MassTransit;

namespace BlueDart.Registration.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Registration";

            var bus = BusConfigurator.ConfigureBus();

            var handle = bus.ConnectReceiveEndpoint(RabbitMqConstants.RegisterOrderServiceQueue, x =>
            {
                x.Consumer<OrderReceivedConsumer>();
            });

            bus.ConnectReceiveEndpoint(RabbitMqConstants.RegisterOrderRollbackServiceQueue, x =>
            {
                x.Consumer<PaymentFailedConsumer>();
            });

            bus.Start();

            Console.WriteLine("Listening for Register/Payment Failed order commands.. Press enter to exit");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
