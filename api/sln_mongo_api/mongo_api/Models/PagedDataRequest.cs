namespace mongo_api.Models
{
    public abstract class PagedDataRequest
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public bool? Active { get; set; }
        public string Column { get; set; }
        public bool Desc { get; set; }

        public PagedDataRequest()
        {
            Page = 1;
            Limit = 30;
            Column = "Active";
            Desc = true;
        }

    }
}
