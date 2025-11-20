using CORE.APP.Domain;

namespace Movies.APP.Domain
{
    public class Movie : Entity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }
        public decimal TotalRevenue { get; set; }

        public int DirectorId { get; set; }
        public Director Director { get; set; }
        
        public List<MovieGenre> MovieGenres { get; set; } = new();

    }
}