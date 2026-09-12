namespace Transportation.Buisness.Services.CompanyFeatures.Dtos
{
    public class CompanyFeatureDetailsDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
