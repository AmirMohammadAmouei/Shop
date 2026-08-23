namespace Transportation.Buisness._0.Common.Constants.Identity
{
    public class AppRole
    {
        public const string Admin = "Admin";

        public static IEnumerable<string> GetAll() =>
            new[] { Admin };
    }
}
