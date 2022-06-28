using Import.Models;

using Microsoft.EntityFrameworkCore;

namespace Import.Context
{
    public class ImportDbContext: DbContext
    {
        public ImportDbContext(DbContextOptions<ImportDbContext> options) : base(options)
        {

        }
        public DbSet<YamlProduct> YamlProducts { get; set; }
    }
}
