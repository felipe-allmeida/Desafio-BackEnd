using BikeRental.Domain.Enums;
using BuildingBlocks.Common;

namespace BikeRental.Domain.ValueObjects
{
    public class CNH : ValueObject
    {
        protected CNH() { }
        public CNH(ECNHType type, string number, string image)
        {
            Type = type;
            Number = number;
            Image = image;
        }

        public ECNHType Type { get; private set; }
        public string Number { get; private set; }
        public string Image { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Number;
            yield return Image;
        }
    }
}
