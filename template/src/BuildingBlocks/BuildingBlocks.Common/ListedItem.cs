namespace BuildingBlocks.Common
{
    public class ListedItem<TResponse> : ValueObject
    {
        public IReadOnlyList<TResponse> Items { get; }

        public ListedItem(IReadOnlyList<TResponse> items)
        {
            Items = items;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Items;
        }
    }
}
