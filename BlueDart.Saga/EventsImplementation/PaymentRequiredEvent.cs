using System;
using BlueDart.Messaging.Events;

namespace BlueDart.Saga
{
    public class PaymentRequiredEvent : IPaymentRequiredEvent
    {
        private readonly OrderSagaState orderSagaState;

        public PaymentRequiredEvent(OrderSagaState orderSagaState)
        {
            this.orderSagaState = orderSagaState;
        }

        public Guid CorrelationId => orderSagaState.CorrelationId;
        public string PickupName => orderSagaState.PickupName;
    }
}