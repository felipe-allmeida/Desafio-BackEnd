using BikeRental.Application.DTOs.V1;
using BikeRental.Domain.Enums;
using BuildingBlocks.Common;
using MediatR;

namespace BikeRental.Application.Queries.V1.Admin.GetBikes
{
    public record GetBikesQuery : IRequest<PaginatedItem<BikeDto>>
    {
        public enum EBikeOrdenationOptions
        {
            CreatedAt,
            UpdatedAt,
            DeletedAt,
        }

        public enum EBikeStatus
        {
            All,
            ActiveOnly,
            InactiveOnly,
        }

        public EBikeOrdenationOptions OrderBy { get; init; } = EBikeOrdenationOptions.CreatedAt;
        public ESortOptions Sort { get; init; } = ESortOptions.Descending;
        public EBikeStatus Status { get; init; } = EBikeStatus.ActiveOnly;
        public string? Plate { get; init; }
        public int? Year { get; init; }
        public DateTimeOffset? Start { get; init; }
        public DateTimeOffset? End { get; init; }
        public int Skip { get; init; } = 0;
        public int Take { get; init; } = 10;
    }
}
