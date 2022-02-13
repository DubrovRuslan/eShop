namespace Catalog.Host.Data
{
    public class SelectedItems<T>
    {
        public long TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}
