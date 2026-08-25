namespace Transportation.Buisness.Services.MobileApps.Dtos
{
    public class MobileAppListResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
        public string FilePath { get; set; }
        public string IconPath { get; set; }
        public long FileSize { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
