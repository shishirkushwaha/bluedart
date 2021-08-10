using System;
using BlueDart.Messaging.Events;
using Automatonymous;
using MassTransit;

namespace BlueDart.Saga
{
    public class OrderSaga : MassTransitStateMachine<OrderSagaState>
    {
        public State Received { get; private set; }
        public State Registered { get; private set; }
        public State Paid { get; private set; }
        public State Failed { get; private set; }
        public State Shipped { get; private set; }

        public Event<IRegisterOrderCommand> RegisterOrder { get; private set; }
        public Event<IOrderRegisteredEvent> OrderRegistered { get; private set; }
        public Event<IPaymentDoneEvent> PaymentDone { get; private set; }
        public Event<IPaymentRequiredEvent> PaymentRequired { get; private set; }
        public Event<IPaymentFailedEvent> PaymentFailed { get; private set; }
        public Event<IOrderShippedEvent> OrderShipped { get; private set; }
        
        public Event<Fault<IPaymentRequiredEvent>> PaymentRequiredFault { get; private set; }
        public Event<Fault<IOrderReceivedEvent>> OrderRegisteredFault { get; private set; }

        public OrderSaga()
        {
            InstanceState(s => s.CurrentState);

            Event(() => RegisterOrder,
                cc =>
                    cc.CorrelateBy(state => state.PickupName, context =>
                        context.Message.PickupName)
                            .SelectId(context => Guid.NewGuid()));

            Event(() => OrderRegistered, x => x.CorrelateById(context =>
                context.Message.CorrelationId));

            Event(() => PaymentRequiredFault, x => x.CorrelateById(context =>
                context.Message.Message.CorrelationId));

            Event(() => OrderRegisteredFault, x => x.CorrelateById(context =>
                context.Message.Message.CorrelationId));

            Event(() => PaymentDone, x => x.CorrelateById(context =>
                context.Message.CorrelationId));

            Event(() => PaymentRequired, x => x.CorrelateById(context =>
                context.Message.CorrelationId));

            Event(() => PaymentFailed, x => x.CorrelateById(context =>
                context.Message.CorrelationId));

            Event(() => OrderShipped, x => x.CorrelateById(context =>
                context.Message.CorrelationId));

            Initially(
                When(RegisterOrder)
                    .Then(context =>
                    {
                        context.Instance.ReceivedDateTime = DateTime.Now;
                        context.Instance.PickupName = context.Data.PickupName;
                        context.Instance.PickupAddress = context.Data.PickupAddress;
                        context.Instance.PickupCity = context.Data.PickupCity;
                        context.Instance.DeliverCity = context.Data.DeliverCity;
                        //Map other Properties as Required
                    })
                    .ThenAsync(
                        context => StatusDispatcher.Dispatch($"Order for customer" + $" {context.Data.PickupName} received from {context.Data.PickupCity}."))
                    .TransitionTo(Received)
                    .Publish(context => new OrderReceivedEvent(context.Instance))
                );

            During(Received,
                When(OrderRegistered)
                    .Then(context => context.Instance.RegisteredDateTime = DateTime.Now)
                    .ThenAsync(
                        context => StatusDispatcher.Dispatch($"Order for customer {context.Instance.PickupName} " + $"registered."))
                    .TransitionTo(Registered)
                    .Publish(context => new PaymentRequiredEvent(context.Instance))
                );

            During(Registered, When(PaymentRequired).Then(context =>
            {
                //Do nothing
            }));

            During(Registered,
                When(PaymentRequiredFault)
                    .ThenAsync(
                        context => StatusDispatcher.Dispatch($"Payment Failed for customer {context.Instance.PickupName}. Faulty Process was {context.Data.Host.ProcessName}."))
                    .TransitionTo(Failed)
                    .Publish(context => new PaymentFailedEvent(context.Instance))
            );

            During(Registered,
                When(PaymentDone)
                    .Then(context => context.Instance.RegisteredDateTime = DateTime.Now)
                    .ThenAsync(
                        context => StatusDispatcher.Dispatch($"Order for Customer {context.Instance.PickupName} is Paid. OrderId : {context.Instance.CorrelationId}"))
                    .TransitionTo(Paid)
                );

            During(Paid,
                When(OrderShipped)
                    .ThenAsync(
                        context => StatusDispatcher.Dispatch($"Order for Customer {context.Instance.PickupName} is Shipped to {context.Instance.DeliverCity}."))
                    .TransitionTo(Shipped)
                    .Finalize()
                );

            SetCompletedWhenFinalized();
        }
    }
}