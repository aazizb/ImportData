using Import.Models;

using Microsoft.EntityFrameworkCore;

namespace Import.Context
{
    public class ImportDbContext: DbContext
    {
        public DbSet<YamlProduct> YamlProducts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //should be stored in a configuration file
            string connectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=importdb;Integrated Security=True";
            optionsBuilder.UseSqlServer(connectionString);

        }
    }
}
