namespace _Common.Fetch.Pagination
{
    public class PaginationResponse<T> where T : class
    {

        public PaginationResponse(int totalCount, int pageSize, int currentPageNumber, List<T> data)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPageNumber = currentPageNumber;
            Data = data;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            HasPreviousPage = CurrentPageNumber > 1;
            HasNextPage = CurrentPageNumber < TotalPages;
        }

        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public List<T> Data { get; set; } = new(0);
    }
}
