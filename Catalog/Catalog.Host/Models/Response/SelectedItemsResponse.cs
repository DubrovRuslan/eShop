namespace Catalog.Host.Models.Response
{
    public class SelectedItemsResponse<T>
    {
        public long Count { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}
