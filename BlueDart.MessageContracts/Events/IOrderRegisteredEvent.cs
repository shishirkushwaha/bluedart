using System;

namespace BlueDart.Messaging.Events
{
    public interface IOrderRegisteredEvent
    {
        Guid CorrelationId { get; }
    }
}
