namespace Kore.CmsApi
{
    public record PagedResult<T>
    {
        public int TotalCount { get; set; }
        public List<T>? Items {  get; set; }
    }
}
