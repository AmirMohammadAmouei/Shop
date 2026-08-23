namespace Transportation.DataAccess.SeedManager.Settings
{
    public class AdminSeedSettings
    {
        public const string SectionName = "AdminSeed";

        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
