using CORE.APP.Models;
using CORE.APP.Services;
using Movies.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Movies
{
    public class MovieCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(250)]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int DirectorId { get; set; }
        
        public List<int> GenreIds { get; set; } = new();  // <-- EKLENDİ

    }

    public class MovieCreateHandler 
        : Service<Movie>, IRequestHandler<MovieCreateRequest, CommandResponse>
    {
        public MovieCreateHandler(MoviesDb db) : base(db) { }

        public async Task<CommandResponse> Handle(MovieCreateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(m => m.Name == request.Name.Trim(), cancellationToken))
                return Error("Movie with the same name exists!");

            var entity = new Movie
            {
                Name = request.Name.Trim(),
                ReleaseDate = request.ReleaseDate,
                TotalRevenue = request.TotalRevenue,
                DirectorId = request.DirectorId
            };

            //böyle yapma tek satır yap 
            if (request.GenreIds != null && request.GenreIds.Any())
            {
                // Add genre relations
                foreach (var genreId in request.GenreIds)
                {
                    entity.MovieGenres.Add(new MovieGenre
                    {
                        GenreId = genreId
                    });
                }
            }
            
            await Create(entity, cancellationToken);

            return Success("Movie created successfully.", entity.Id);
        }
    }
}