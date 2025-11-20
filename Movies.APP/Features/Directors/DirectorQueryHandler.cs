using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Directors
{
    public class DirectorQueryRequest : Request, IRequest<IQueryable<DirectorQueryResponse>> {}

    public class DirectorQueryResponse : Response
    {
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public bool IsRetired { get; set; }
    }

    public class DirectorQueryHandler : Service<Director>,
        IRequestHandler<DirectorQueryRequest, IQueryable<DirectorQueryResponse>>
    {
        public DirectorQueryHandler(MoviesDb db) : base(db) {}

        public Task<IQueryable<DirectorQueryResponse>> Handle(DirectorQueryRequest request,
            CancellationToken cancellationToken)
        {
            var query =
                Query()
                    .OrderBy(d => d.LastName)
                    .ThenBy(d => d.FirstName)
                    .Select(d => new DirectorQueryResponse
                    {
                        Id = d.Id,
                        Guid = d.Guid,
                        FirstName = d.FirstName,
                        LastName  = d.LastName,
                        IsRetired = d.IsRetired
                    });

            return Task.FromResult(query);
        }
    }
}