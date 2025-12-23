using CORE.APP.Models;
using CORE.APP.Services;
using Movies.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Movies.APP.Features.Movies
{
    public class MovieDeleteRequest : Request, IRequest<CommandResponse> { }

    public class MovieDeleteHandler 
        : Service<Movie>, IRequestHandler<MovieDeleteRequest, CommandResponse>
    {
        public MovieDeleteHandler(MoviesDb db) : base(db) { }

        public async Task<CommandResponse> Handle(MovieDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false)
                .SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Movie not found!");

            Delete(entity.MovieGenres);
            await Delete(entity, cancellationToken);

            return Success("Movie deleted successfully.", entity.Id);
        }
    }
}