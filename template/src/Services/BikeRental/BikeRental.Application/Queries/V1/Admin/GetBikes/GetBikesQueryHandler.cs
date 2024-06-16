using BikeRental.Application.DTOs.V1;
using BikeRental.Application.Extensions;
using BikeRental.Domain.Enums;
using BikeRental.Domain.Models.BikeAggregate;
using BuildingBlocks.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static BikeRental.Application.Queries.V1.Admin.GetBikes.GetBikesQuery;

namespace BikeRental.Application.Queries.V1.Admin.GetBikes
{
    public class GetBikesQueryHandler(IBikeQueryRepository queryRepository) : IRequestHandler<GetBikesQuery, PaginatedItem<BikeDto>>
    {
        private readonly IBikeQueryRepository _queryRepository = queryRepository ?? throw new ArgumentNullException(nameof(queryRepository));
        public async Task<PaginatedItem<BikeDto>> Handle(GetBikesQuery request, CancellationToken cancellationToken)
        {
            var queryable = _queryRepository.GetAll().IgnoreQueryFilters();

            switch (request.Status)
            {
                case GetBikesQuery.EBikeStatus.ActiveOnly:
                    queryable = queryable.Where(x => x.IsDeleted == false);
                    break;
                case GetBikesQuery.EBikeStatus.InactiveOnly:
                    queryable = queryable.Where(x => x.IsDeleted == true);
                    break;
            }

            switch (request.OrderBy)
            {
                case GetBikesQuery.EBikeOrdenationOptions.CreatedAt:
                    queryable = ApplyDateFilters(queryable, request.Start, request.End, x => x.CreatedAt);
                    break;
                case GetBikesQuery.EBikeOrdenationOptions.UpdatedAt:
                    queryable = ApplyDateFilters(queryable, request.Start, request.End, x => x.UpdatedAt);
                    break;
                case GetBikesQuery.EBikeOrdenationOptions.DeletedAt:
                    queryable = ApplyDateFilters(queryable, request.Start, request.End, x => x.DeletedAt);
                    break;
            }

            if (request.Year.HasValue)
            {
                queryable = queryable.Where(x => x.Year == request.Year);
            }

            if (!string.IsNullOrEmpty(request.Plate))
            {
                queryable = queryable.Where(x => EF.Functions.Like(x.Plate, $"%{request.Plate}%"));
            }

            var ordenation = OrderAndSortQuery(request.OrderBy, request.Sort);

            return await queryable
                .Select(x => new BikeDto
                {
                    Id = x.Id,
                    Plate = x.Plate,
                    Model = x.Model,
                    Year = x.Year,
                    IsDeleted = x.IsDeleted,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                })
                .PaginateAsync(request.Skip, request.Take);
        }

        private static IQueryable<Bike> ApplyDateFilters(IQueryable<Bike> queryable, DateTimeOffset? start, DateTimeOffset? end, Func<Bike, DateTimeOffset?> dateSelector)
        {
            if (start.HasValue)
            {
                var startDate = new DateTime(start.Value.Year, start.Value.Month, start.Value.Day, 0, 0, 0);
                queryable = queryable.Where(x => dateSelector(x) >= startDate);
            }
            if (end.HasValue)
            {
                var endDate = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day, 23, 59, 59);
                queryable = queryable.Where(x => dateSelector(x) <= endDate);
            }

            return queryable;
        }

        private static Func<IQueryable<BikeDto>, IOrderedQueryable<BikeDto>> OrderAndSortQuery(EBikeOrdenationOptions order, ESortOptions? sort)
        {
            Func<IQueryable<BikeDto>, IOrderedQueryable<BikeDto>> orderBy = p => p.OrderByDescending(n => n.CreatedAt);

            switch (order)
            {
                case EBikeOrdenationOptions.CreatedAt:
                    orderBy = x => sort == ESortOptions.Ascending ? x.OrderBy(n => n.CreatedAt) : x.OrderByDescending(n => n.CreatedAt);
                    break;
                case EBikeOrdenationOptions.UpdatedAt:
                    orderBy = x => sort == ESortOptions.Ascending ? x.OrderBy(n => n.UpdatedAt) : x.OrderByDescending(n => n.UpdatedAt);
                    break;
                case EBikeOrdenationOptions.DeletedAt:
                    orderBy = x => sort == ESortOptions.Ascending ? x.OrderBy(n => n.DeletedAt) : x.OrderByDescending(n => n.DeletedAt);
                    break;
            }
            return orderBy;
        }
    }
}
