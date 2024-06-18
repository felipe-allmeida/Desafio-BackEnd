using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.Application.Commands.V1.Automated.NotifyDeliveryRequestToAvaiableDeliveryRiders
{
    public record NotifyDeliveryRequestToAvaiableDeliveryRidersCommand : IRequest
    {
        public Guid DeliveryRequestId { get; init; }
        public int Status { get; init; }
        public int PriceCents { get; init; }
    }
}
