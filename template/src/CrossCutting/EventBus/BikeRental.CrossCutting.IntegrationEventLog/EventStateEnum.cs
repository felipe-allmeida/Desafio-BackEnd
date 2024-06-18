using System.Collections.Generic;
using System.Reflection.Emit;

namespace BikeRental.CrossCutting.IntegrationEventLog
{
    public enum EventStateEnum
    {
        NotPublished = 0,
        InProgress = 1,
        Published = 2,
        PublishedFailed = 3
    }
}
