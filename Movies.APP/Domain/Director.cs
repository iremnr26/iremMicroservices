using CORE.APP.Domain;

namespace Movies.APP.Domain
{
    public class Director : Entity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;
        public bool IsRetired { get; set; }

        public List<Movie> Movies { get; set; } = new();
    }
}