using System;

namespace BlueDart.Messaging.Events
{
    public interface IPaymentFailedEvent
    {
        Guid CorrelationId { get; }
        string PickupName { get; }
    }
}
