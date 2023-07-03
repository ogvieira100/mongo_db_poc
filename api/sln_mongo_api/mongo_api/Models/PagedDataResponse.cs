namespace mongo_api.Models
{
    public class PagedDataResponse<T> where T : class
    {

        public List<T> Items { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public long TotalItens { get; set; }

        public int TotalPages { get; set; }

        public PagedDataResponse()
        {
            Items = new List<T>();
        }

    }
}
