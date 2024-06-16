using BuildingBlocks.Common.Extensions;
using System.Text.Json;

namespace BikeRental.API.Infrastructure.Serialization
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToSnakeCase();
    }

}
