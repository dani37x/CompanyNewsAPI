using Microsoft.EntityFrameworkCore;
using NexteerNewsAPI.Models;

namespace CompanyNewsAPI.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<CompanyNews> News => Set<CompanyNews>();

    }
}
