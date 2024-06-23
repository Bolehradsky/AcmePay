namespace _Common.Fetch.Pagination
{
    public class PaginationRequest
    {
        public PaginationRequest()
        {
        }

        public PaginationRequest(int page, int size)
        {
            this.CurrentPage = page;
            this.PageSize = size;
        }

        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}

