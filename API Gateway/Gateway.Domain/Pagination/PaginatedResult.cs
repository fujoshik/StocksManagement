namespace Gateway.Domain.Pagination
{
    public record PaginatedResult<T>
    {
        public List<T> Content { get; set; }
        public int NumberOfElements { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public static PaginatedResult<T> EmptyResult(int pageNumber)
            => new(new PaginatedResult<T>(new List<T>(), pageNumber, 0));

        public PaginatedResult(List<T> content, int pageNumber, int pageSize)
        {
            Content = content;
            NumberOfElements = content.Count;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
