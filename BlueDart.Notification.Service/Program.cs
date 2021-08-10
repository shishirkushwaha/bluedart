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
            Console.Title = "Notification";

            var bus = BusConfigurator.ConfigureBus();

            var handle = bus.ConnectReceiveEndpoint(RabbitMqConstants.NotificationServiceQueue, x =>
            {
                x.Consumer<OrderRegisteredConsumer>();
            });

            bus.Start();

            Console.WriteLine("Listening for Order registered events.. Press enter to exit");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
