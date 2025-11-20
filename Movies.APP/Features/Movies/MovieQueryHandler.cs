using CORE.APP.Models;
using CORE.APP.Services;
using Movies.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Movies.APP.Features.Movies
{
    public class MovieQueryRequest : Request, IRequest<IQueryable<MovieQueryResponse>>
    {
       // public int? DirectorId { get; set; }
    }

    public class MovieQueryResponse : Response
    {
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public string DirectorFullName { get; set; }

        public List<string> Genres { get; set; } = new();
    }


    public class MovieQueryHandler 
        : Service<Movie>, IRequestHandler<MovieQueryRequest, IQueryable<MovieQueryResponse>>
    {
        public MovieQueryHandler(MoviesDb db) : base(db) { }

        protected override IQueryable<Movie> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(m => m.Director)
                .Include(m=> m.MovieGenres).ThenInclude(m => m.Genre);
        }


        public Task<IQueryable<MovieQueryResponse>> Handle(MovieQueryRequest request, CancellationToken cancellationToken)
        {
            var entityQuery = Query().Select(m => new MovieQueryResponse
            {
                Id = m.Id,
                Name = m.Name,
                ReleaseDate = m.ReleaseDate,
                TotalRevenue = m.TotalRevenue,
                DirectorFullName = m.Director.FirstName + " " + m.Director.LastName,
                Genres = m.MovieGenres
                    .Select(mg=>mg.Genre.Name)
                    .ToList()
            });
            return Task.FromResult(entityQuery);
        }
    }
}