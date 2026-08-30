using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Transportation.DataAccess.Contexts
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=Amir\\Amir;Database=Transportation;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
    

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
