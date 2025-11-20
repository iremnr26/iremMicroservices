using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Genres
{
    public class GenreQueryRequest : Request, IRequest<IQueryable<GenreQueryResponse>>
    {
    }

    public class GenreQueryResponse : Response
    {
        public string Name { get; set; }
    }

    public class GenreQueryHandler : Service<Genre>,
        IRequestHandler<GenreQueryRequest, IQueryable<GenreQueryResponse>>
    {
        public GenreQueryHandler(MoviesDb db) : base(db) {}

        public Task<IQueryable<GenreQueryResponse>> Handle(GenreQueryRequest request,
            CancellationToken cancellationToken)
        {
            var query = Query()
                .OrderBy(g => g.Name)
                .Select(g => new GenreQueryResponse
                {
                    Id = g.Id,
                    Guid = g.Guid,
                    Name = g.Name
                });

            return Task.FromResult(query);
        }
    }
}