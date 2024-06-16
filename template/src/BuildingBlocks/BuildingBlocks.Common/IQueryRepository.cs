namespace BuildingBlocks.Common
{
    public interface IQueryRepository
    {

    }

    public interface IQueryRepository<T> : IQueryRepository where T : class
    {
        IQueryable<T> GetAll();
    }

}
