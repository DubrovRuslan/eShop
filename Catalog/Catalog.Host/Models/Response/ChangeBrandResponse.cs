namespace Catalog.Host.Models.Response
{
    public class ChangeBrandResponse<T>
    {
        public T Result { get; set; } = default(T) !;
    }
}
