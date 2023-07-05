using Microsoft.EntityFrameworkCore;
using NexteerNewsAPI.Models;

namespace NexteerNewsAPI.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<CompanyNews> News => Set<CompanyNews>();

    }
}
