using CORE.APP.Domain;

namespace Movies.APP.Domain
{
    public class Genre : Entity
    {
        public string Name { get; set; }

        public List<MovieGenre> MovieGenres { get; set; } = new();
    }
}