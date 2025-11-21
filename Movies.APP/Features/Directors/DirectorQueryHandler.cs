using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Directors
{
    /// <summary>
    /// Query request for fetching directors.
    /// </summary>
    public class DirectorQueryRequest : Request, 
        IRequest<IQueryable<DirectorQueryResponse>> {}

    /// <summary>
    /// Query response DTO returned to controller.
    /// Inherits from Response for common fields: Id, Guid.
    /// </summary>
    public class DirectorQueryResponse : Response
    {
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public bool IsRetired { get; set; }
    }

    /// <summary>
    /// Handler that processes DirectorQueryRequest and returns
    /// an IQueryable list of directors, ordered and projected
    /// into DirectorQueryResponse DTO.
    /// </summary>
    public class DirectorQueryHandler : Service<Director>,
        IRequestHandler<DirectorQueryRequest, IQueryable<DirectorQueryResponse>>
    {
        /// <summary>
        /// Constructor: injects DbContext and forwards it
        /// to Service base class.
        /// </summary>
        public DirectorQueryHandler(MoviesDb db) : base(db) {}

        /// <summary>
        /// Handles the query:
        /// 1. Gets director table via Query()
        /// 2. Applies ordering (LastName → FirstName)
        /// 3. Maps entity fields into DirectorQueryResponse DTO
        /// 4. Returns IQueryable to the controller
        ///
        /// IMPORTANT:
        /// - IQueryable is returned so controller can apply ToListAsync()
        /// - AsNoTracking is used (Query()) because this is read-only
        /// </summary>
        public Task<IQueryable<DirectorQueryResponse>> Handle(
            DirectorQueryRequest request,
            CancellationToken cancellationToken)
        {
            // ======================================================
            // Query()
            // → AsNoTracking IQueryable<Director> (read-only, fast)
            // ======================================================
            var query =
                Query()
                    // Sort directors by LastName then FirstName
                    .OrderBy(d => d.LastName)
                    .ThenBy(d => d.FirstName)

                    // Project entity → response DTO
                    .Select(d => new DirectorQueryResponse
                    {
                        Id        = d.Id,
                        Guid      = d.Guid,
                        FirstName = d.FirstName,
                        LastName  = d.LastName,
                        IsRetired = d.IsRetired
                    });

            // Returning IQueryable - NOT executing yet.
            return Task.FromResult(query);
        }
    }
}
