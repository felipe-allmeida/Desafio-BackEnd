namespace BuildingBlocks.Common
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IUnitOfWork UnitOfWork { get; }
    }

}
