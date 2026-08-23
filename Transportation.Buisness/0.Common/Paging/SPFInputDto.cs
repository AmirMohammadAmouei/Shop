namespace Transportation.Buisness._0.Common.Paging
{
    public class SPFInputDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
    }

}
