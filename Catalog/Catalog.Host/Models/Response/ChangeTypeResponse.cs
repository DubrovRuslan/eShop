namespace Catalog.Host.Models.Response
{
    public class ChangeTypeResponse<T>
    {
        public T Result { get; set; } = default(T) !;
    }
}
