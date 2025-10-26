namespace Users.APP.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


    public class UsersDbFactory : IDesignTimeDbContextFactory<UsersDb>
    {
        /// <summary>
        /// The connection string in the configuration file (appsettings.json).
        /// </summary>
        const string CONNECTIONSTRING = "data source=UsersDB";

        /// <summary>
        /// Creates a new instance of the <see cref="UsersDb"/> context using the connection string.
        /// This method is called by EF Core tooling at design time.
        /// </summary>
        /// <param name="args">Command-line arguments (not used).</param>
        /// <returns>A configured <see cref="UsersDb"/> instance.</returns>
        public UsersDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersDb>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new UsersDb(optionsBuilder.Options);
        }
    }
