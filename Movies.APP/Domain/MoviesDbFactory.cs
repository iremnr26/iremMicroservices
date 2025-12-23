using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Movies.APP.Domain
{
    //EF Core migration oluştururken veya
    //database update yaparken uygulamayı çalıştırmadan DbContext’i nasıl oluşturacağını bilsin
    public class MoviesDbFactory : IDesignTimeDbContextFactory<MoviesDb>
    {
        public MoviesDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MoviesDb>();

            // SQLite Connection 
            optionsBuilder.UseSqlite("Data Source=MoviesDB.db");

            return new MoviesDb(optionsBuilder.Options);
        }
    }
}