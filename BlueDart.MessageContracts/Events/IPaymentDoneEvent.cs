using System;

namespace BlueDart.Messaging.Events
{
    public interface IPaymentDoneEvent
    {
        Guid CorrelationId { get; }
    }
}
