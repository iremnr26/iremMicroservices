using Microsoft.EntityFrameworkCore;

namespace Movies.APP.Domain
{
    public class MoviesDb : DbContext
    {
        //relationlar burada kontrol ediiyor
        public MoviesDb(DbContextOptions<MoviesDb> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedGenres(modelBuilder);
            SeedDirectors(modelBuilder);
            SeedMovies(modelBuilder);
            SeedMovieGenres(modelBuilder);
            // -------------------------
            // UNIQUE CONSTRAINTS
            // -------------------------

            // Film isimleri benzersiz olmalı
            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.Name)
                .IsUnique();

            // Genre isimleri benzersiz
            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.Name)
                .IsUnique();

            // Yönetmen adı + soyadı benzersiz olmalı (Mantıklı bir constraint)
            modelBuilder.Entity<Director>()
                .HasIndex(d => new { d.FirstName, d.LastName })
                .IsUnique();


            // -------------------------
            // RELATIONSHIPS
            // -------------------------

            // Movie -> Director (1 Director, many Movies)
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.NoAction);//cascade kullanmıyoruz bağlantılı table gittiğinde komple gitmsein diye

            // MovieGenre -> Movie (many to one)
            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            // MovieGenre -> Genre (many to one)
            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId)
                .OnDelete(DeleteBehavior.NoAction);

            // -------------------------
            // COMPOSITE UNIQUE (MovieId + GenreId)
            // Aynı filmin aynı genre kaydı iki kez eklenemez.
            // -------------------------
            modelBuilder.Entity<MovieGenre>()
                .HasIndex(mg => new { mg.MovieId, mg.GenreId })
                .IsUnique();
        }
        
        private void SeedGenres(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Guid = Guid.NewGuid().ToString(), Name = "Sci-Fi" },
                new Genre { Id = 2, Guid = Guid.NewGuid().ToString(), Name = "Drama" },
                new Genre { Id = 3, Guid = Guid.NewGuid().ToString(), Name = "Thriller" },
                new Genre { Id = 4, Guid = Guid.NewGuid().ToString(), Name = "Adventure" },
                new Genre { Id = 5, Guid = Guid.NewGuid().ToString(), Name = "Action" },
                new Genre { Id = 6, Guid = Guid.NewGuid().ToString(), Name = "Fantasy" },
                new Genre { Id = 7, Guid = Guid.NewGuid().ToString(), Name = "Mystery" },
                new Genre { Id = 8, Guid = Guid.NewGuid().ToString(), Name = "Comedy" },
                new Genre { Id = 9, Guid = Guid.NewGuid().ToString(), Name = "Horror" },
                new Genre { Id = 10, Guid = Guid.NewGuid().ToString(), Name = "Crime" }
            );
        }

        private void SeedDirectors(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Director>().HasData(
                new Director { Id = 1, Guid = Guid.NewGuid().ToString(), FirstName = "Christopher", LastName = "Nolan", IsRetired = false },
                new Director { Id = 2, Guid = Guid.NewGuid().ToString(), FirstName = "Steven", LastName = "Spielberg", IsRetired = false },
                new Director { Id = 3, Guid = Guid.NewGuid().ToString(), FirstName = "James", LastName = "Cameron", IsRetired = false },
                new Director { Id = 4, Guid = Guid.NewGuid().ToString(), FirstName = "Quentin", LastName = "Tarantino", IsRetired = false },
                new Director { Id = 5, Guid = Guid.NewGuid().ToString(), FirstName = "David", LastName = "Fincher", IsRetired = false },
                new Director { Id = 6, Guid = Guid.NewGuid().ToString(), FirstName = "Peter", LastName = "Jackson", IsRetired = false },
                new Director { Id = 7, Guid = Guid.NewGuid().ToString(), FirstName = "Denis", LastName = "Villeneuve", IsRetired = false },
                new Director { Id = 8, Guid = Guid.NewGuid().ToString(), FirstName = "Jordan", LastName = "Peele", IsRetired = false },
                new Director { Id = 9, Guid = Guid.NewGuid().ToString(), FirstName = "Martin", LastName = "Scorsese", IsRetired = false },
                new Director { Id = 10, Guid = Guid.NewGuid().ToString(), FirstName = "Ridley", LastName = "Scott", IsRetired = false }
            );
        }

        private void SeedMovies(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Guid = Guid.NewGuid().ToString(), Name = "Inception", ReleaseDate = new DateTime(2010, 7, 16), TotalRevenue = 829_000_000, DirectorId = 1 },
                new Movie { Id = 2, Guid = Guid.NewGuid().ToString(), Name = "Interstellar", ReleaseDate = new DateTime(2014, 11, 7), TotalRevenue = 677_000_000, DirectorId = 1 },
                new Movie { Id = 3, Guid = Guid.NewGuid().ToString(), Name = "Avatar", ReleaseDate = new DateTime(2009, 12, 18), TotalRevenue = 2_923_000_000, DirectorId = 3 },
                new Movie { Id = 4, Guid = Guid.NewGuid().ToString(), Name = "Pulp Fiction", ReleaseDate = new DateTime(1994, 10, 14), TotalRevenue = 214_000_000, DirectorId = 4 },
                new Movie { Id = 5, Guid = Guid.NewGuid().ToString(), Name = "Fight Club", ReleaseDate = new DateTime(1999, 10, 15), TotalRevenue = 101_000_000, DirectorId = 5 },
                new Movie { Id = 6, Guid = Guid.NewGuid().ToString(), Name = "LOTR: Fellowship", ReleaseDate = new DateTime(2001, 12, 19), TotalRevenue = 880_000_000, DirectorId = 6 },
                new Movie { Id = 7, Guid = Guid.NewGuid().ToString(), Name = "Get Out", ReleaseDate = new DateTime(2017, 2, 24), TotalRevenue = 255_000_000, DirectorId = 8 },
                new Movie { Id = 8, Guid = Guid.NewGuid().ToString(), Name = "Shutter Island", ReleaseDate = new DateTime(2010, 2, 19), TotalRevenue = 295_000_000, DirectorId = 9 },
                new Movie { Id = 9, Guid = Guid.NewGuid().ToString(), Name = "Gladiator", ReleaseDate = new DateTime(2000, 5, 5), TotalRevenue = 503_000_000, DirectorId = 10 },
                new Movie { Id = 10, Guid = Guid.NewGuid().ToString(), Name = "Jurassic Park", ReleaseDate = new DateTime(1993, 6, 11), TotalRevenue = 1_030_000_000, DirectorId = 2 }
            );
        }

        private void SeedMovieGenres(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>().HasData(
                new MovieGenre { Id = 1, Guid = Guid.NewGuid().ToString(), MovieId = 1, GenreId = 1 },
                new MovieGenre { Id = 2, Guid = Guid.NewGuid().ToString(), MovieId = 1, GenreId = 3 },

                new MovieGenre { Id = 3, Guid = Guid.NewGuid().ToString(), MovieId = 2, GenreId = 1 },
                new MovieGenre { Id = 4, Guid = Guid.NewGuid().ToString(), MovieId = 2, GenreId = 2 },

                new MovieGenre { Id = 5, Guid = Guid.NewGuid().ToString(), MovieId = 3, GenreId = 1 },
                new MovieGenre { Id = 6, Guid = Guid.NewGuid().ToString(), MovieId = 3, GenreId = 5 },

                new MovieGenre { Id = 7, Guid = Guid.NewGuid().ToString(), MovieId = 4, GenreId = 10 },

                new MovieGenre { Id = 8, Guid = Guid.NewGuid().ToString(), MovieId = 5, GenreId = 10 },
                new MovieGenre { Id = 9, Guid = Guid.NewGuid().ToString(), MovieId = 5, GenreId = 3 },

                new MovieGenre { Id = 10, Guid = Guid.NewGuid().ToString(), MovieId = 6, GenreId = 6 },
                new MovieGenre { Id = 11, Guid = Guid.NewGuid().ToString(), MovieId = 6, GenreId = 4 },

                new MovieGenre { Id = 12, Guid = Guid.NewGuid().ToString(), MovieId = 7, GenreId = 9 },

                new MovieGenre { Id = 13, Guid = Guid.NewGuid().ToString(), MovieId = 8, GenreId = 7 },

                new MovieGenre { Id = 14, Guid = Guid.NewGuid().ToString(), MovieId = 9, GenreId = 5 },

                new MovieGenre { Id = 15, Guid = Guid.NewGuid().ToString(), MovieId = 10, GenreId = 4 }
            );
        }
    }
}
