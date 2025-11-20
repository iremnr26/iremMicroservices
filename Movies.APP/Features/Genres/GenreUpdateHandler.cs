using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Genres
{
    public class GenreUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(150)]
        public string Name { get; set; }
    }

    public class GenreUpdateHandler : Service<Genre>,
        IRequestHandler<GenreUpdateRequest, CommandResponse>
    {
        public GenreUpdateHandler(MoviesDb db) : base(db) {}

        public async Task<CommandResponse> Handle(GenreUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(g =>
                        g.Id != request.Id && g.Name == request.Name.Trim(),
                    cancellationToken))
            {
                return Error("Another genre with the same name exists!");
            }

            var entity = await Query(false)
                .SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Genre not found!");

            entity.Name = request.Name.Trim();

            Update(entity);

            return Success("Genre updated successfully.", entity.Id);
        }
    }
}