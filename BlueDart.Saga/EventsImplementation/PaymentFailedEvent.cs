using System;
using BlueDart.Messaging.Events;

namespace BlueDart.Saga
{
    public class PaymentFailedEvent : IPaymentFailedEvent
    {
        private readonly OrderSagaState orderSagaState;

        public PaymentFailedEvent(OrderSagaState orderSagaState)
        {
            this.orderSagaState = orderSagaState;
        }

        public Guid CorrelationId => orderSagaState.CorrelationId;
        public string PickupName => orderSagaState.PickupName;
    }
}