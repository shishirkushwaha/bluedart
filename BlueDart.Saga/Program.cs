using System;
using BlueDart.Messaging;
using MassTransit;
using MassTransit.Saga;

namespace BlueDart.Saga
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Saga";
            
            var saga = new OrderSaga();
            var repo = new InMemorySagaRepository<OrderSagaState>();

            var bus = BusConfigurator.ConfigureBus();

            var handle = bus.ConnectReceiveEndpoint(RabbitMqConstants.SagaQueue, x =>
            {
                x.StateMachineSaga(saga, repo);
            });

            bus.Start();
            Console.WriteLine("Saga active.. Press enter to exit");
            Console.ReadLine();
            bus.Stop();
        }
    }
}