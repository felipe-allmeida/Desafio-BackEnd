using BikeRental.Domain.ValueObjects;
using BuildingBlocks.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.Domain.Models.DeliveryRiderAggregate
{
    public class DeliveryRider : AggregateRoot<long>
    {
        public string Name { get; private set; }

        public CNPJ CNPJ { get; private set; }
        public CNH CNH { get; private set; }
        public DateTimeOffset Birthday { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }
    }
}
