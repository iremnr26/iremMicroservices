using CORE.APP.Models;
using CORE.APP.Services;
using Movies.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Movies
{
    public class MovieUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(250)]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int DirectorId { get; set; }
        public List<int> GenreIds { get; set; } = new();  // <-- EKLENDİ

    }

    public class MovieUpdateHandler 
        : Service<Movie>, IRequestHandler<MovieUpdateRequest, CommandResponse>
    {
        public MovieUpdateHandler(MoviesDb db) : base(db) { }

        public async Task<CommandResponse> Handle(MovieUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(m => m.Id != request.Id && m.Name == request.Name.Trim(), cancellationToken))
                return Error("Movie with the same name exists!");

            var entity = await Query(false)
                .SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Movie not found!");

            Delete(entity.MovieGenres);

            entity.Name = request.Name.Trim();
            entity.ReleaseDate = request.ReleaseDate;
            entity.TotalRevenue = request.TotalRevenue;
            entity.DirectorId = request.DirectorId;
            
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
            
            await Update(entity, cancellationToken);

            return Success("Movie updated successfully.", entity.Id);
        }
    }
}