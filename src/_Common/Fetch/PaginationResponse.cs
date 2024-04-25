namespace _Common.Fetch
{
    public class PaginationResponse<T> where T : class
    {
        public PaginationResponse(int totalCount, int pageSize, int currentPageNumber, T data)
        {
            this.TotalCount = totalCount;
            this.PageSize = pageSize;
            this.CurrentPageNumber = currentPageNumber;
            this.Data = data;
            TotalPages = (int)Math.Ceiling((double)totalCount / (double)pageSize);
            HasPreviousPage = CurrentPageNumber > 1;
            HasNextPage = CurrentPageNumber < TotalPages;
        }

        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public T Data { get; set; }
    }
}
