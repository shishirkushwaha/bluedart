using System;

namespace BlueDart.Messaging.Events
{
    public interface IReadyToShipEvent
    {
        Guid CorrelationId { get; }
    }
}
