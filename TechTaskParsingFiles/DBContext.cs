using Microsoft.EntityFrameworkCore;
using TechTaskParsingFiles.Models;

namespace TechTaskParsingFiles
{
    public class DBContext:DbContext
    {
        public DBContext() { }
        public DBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<JsonModel> jsons { get; set; }

    }
}
