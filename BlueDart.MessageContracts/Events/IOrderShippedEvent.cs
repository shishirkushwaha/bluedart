using System;

namespace BlueDart.Messaging.Events
{
    public interface IOrderShippedEvent
    {
        Guid CorrelationId { get; }
    }
}
