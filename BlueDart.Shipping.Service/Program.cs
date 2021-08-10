using System;
using BlueDart.Messaging;
using MassTransit;
using GreenPipes;

namespace BlueDart.Notification.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Shipping";

            var bus = BusConfigurator.ConfigureBus();

            var handle = bus.ConnectReceiveEndpoint(RabbitMqConstants.ShippingServiceQueue, x =>
            {
                x.Consumer<PaymentDoneConsumer>();
            });

            bus.Start();

            Console.WriteLine("Listening for Payment Done events.. Press enter to exit");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
