using CompanyNewsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyNewsAPI.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Author> Authors => Set<Author>();

    }
}
