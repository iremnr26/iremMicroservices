using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Directors
{
    public class DirectorDeleteRequest : Request, IRequest<CommandResponse> {}

    public class DirectorDeleteHandler : Service<Director>,
        IRequestHandler<DirectorDeleteRequest, CommandResponse>
    {
        public DirectorDeleteHandler(MoviesDb db) : base(db) {}

        public async Task<CommandResponse> Handle(DirectorDeleteRequest request,
            CancellationToken cancellationToken)
        {
            var entity = await Query(false)
                .SingleOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Director not found!");

            // Eğer yönetmenin Filmi varsa silinmesin (hocanın GroupDelete mantığı gibi)
            var hasMovies = Query<Movie>().Any(m => m.DirectorId == entity.Id);

            if (hasMovies)
                return Error("You cannot delete this director because they have movies!");

            Delete(entity);

            return Success("Director deleted successfully.", entity.Id);
        }
    }
}