using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Directors
{
    /// <summary>
    /// DTO / Command model for creating a new Director.
    /// This object is sent from the Controller to the MediatR pipeline.
    /// </summary>
    public class DirectorCreateRequest : Request, IRequest<CommandResponse>
    {
        /// <summary>
        /// First name of the director.
        /// Required: cannot be null or empty.
        /// Max length: 100 characters.
        /// </summary>
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the director.
        /// Required: cannot be null or empty.
        /// Max length: 100 characters.
        /// </summary>
        [Required, StringLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// Whether the director is retired or still active.
        /// </summary>
        public bool IsRetired { get; set; }
    }

    /// <summary>
    /// Handles the creation of a new Director.
    /// - Inherits from Service<Director> to access Query(), Create(), Success(), Error().
    /// - Implements IRequestHandler to handle DirectorCreateRequest messages.
    /// </summary>
    public class DirectorCreateHandler : Service<Director>,
        IRequestHandler<DirectorCreateRequest, CommandResponse>
    {
        /// <summary>
        /// Constructor: receives MoviesDb via DI and sends it to Service base class.
        /// </summary>
        public DirectorCreateHandler(MoviesDb db) : base(db) {}

        /// <summary>
        /// Handles the creation logic:
        /// 1. Checks for duplicates (FirstName + LastName).
        /// 2. Creates the Director entity.
        /// 3. Saves it to the database.
        /// 4. Returns a typed CommandResponse.
        /// </summary>
        public async Task<CommandResponse> Handle(
            DirectorCreateRequest request,
            CancellationToken cancellationToken)
        {
            // ================================================
            // 1) Duplicate check (Aynı isimde yönetmen var mı?)
            // ================================================

            bool exists = await Query() // AsNoTracking IQueryable<Director>
                .AnyAsync(
                    d =>
                        d.FirstName == request.FirstName.Trim()
                        && d.LastName == request.LastName.Trim(),
                    cancellationToken);

            if (exists)
            {
                // ServiceBase.Error() → unified error response
                return Error("This director already exists!");
            }

            // ================================================
            // 2) Entity oluşturma
            // ================================================

            var entity = new Director
            {
                FirstName = request.FirstName.Trim(),
                LastName  = request.LastName.Trim(),
                IsRetired = request.IsRetired
            };

            // ================================================
            // 3) Veritabanına kaydet
            // Create(entity) → Guid set eder + Add + SaveChanges
            // ================================================

            Create(entity);   // synchronous version → SaveChanges() içeriyor

            // ================================================
            // 4) Başarılı response döndür
            // Success() → (true, message, id)
            // ================================================

            return Success("Director created successfully.", entity.Id);
        }
    }
}
