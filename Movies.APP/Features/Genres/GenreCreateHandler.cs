using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Genres
{
    public class GenreCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(150)]
        public string Name { get; set; }
    }

    public class GenreCreateHandler : Service<Genre>,
        IRequestHandler<GenreCreateRequest, CommandResponse>
    {
        public GenreCreateHandler(MoviesDb db) : base(db) {}

        public async Task<CommandResponse> Handle(GenreCreateRequest request,
            CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(g => g.Name == request.Name.Trim(), cancellationToken))
                return Error("Genre already exists!");

            var entity = new Genre
            {
                Name = request.Name.Trim()
            };

            Create(entity);

            return Success("Genre created successfully.", entity.Id);
        }
    }
}