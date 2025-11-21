using CORE.APP.Domain;

namespace Movies.APP.Domain
{
    public class MovieGenre : Entity
    { 
        public MovieGenre()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}