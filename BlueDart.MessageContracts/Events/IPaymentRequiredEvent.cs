using System;

namespace BlueDart.Messaging.Events
{
    public interface IPaymentRequiredEvent
    {
        Guid CorrelationId { get; }
        string PickupName { get; }
    }
}
