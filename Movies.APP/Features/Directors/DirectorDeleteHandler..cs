using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Directors
{
    /// <summary>
    /// Request object for deleting a director.
    /// Inherits from Request and implements IRequest to work with MediatR.
    /// Only contains the Id of the director to delete (inherited from Request).
    /// </summary>
    public class DirectorDeleteRequest : Request, IRequest<CommandResponse> { }

    /// <summary>
    /// Handles the deletion of a director.
    /// - Uses Service<Director> to access shared CRUD and query methods.
    /// - Implements MediatR IRequestHandler for DirectorDeleteRequest.
    /// </summary>
    public class DirectorDeleteHandler : Service<Director>,
        IRequestHandler<DirectorDeleteRequest, CommandResponse>
    {
        /// <summary>
        /// Injects MoviesDb and sends it to the base Service class.
        /// </summary>
        public DirectorDeleteHandler(MoviesDb db) : base(db) {}

        /// <summary>
        /// Main delete logic:
        /// 1. Finds director by ID.
        /// 2. Checks if director exists.
        /// 3. Checks if director has movies.
        /// 4. Deletes director if allowed.
        /// </summary>
        public async Task<CommandResponse> Handle(
            DirectorDeleteRequest request,
            CancellationToken cancellationToken)
        {
            // =====================================================
            // 1) Director'ı ID ile bul → Tracking açık (false)
            // Tracking açık olmalı çünkü Delete(entity) update tracking kullanır.
            // =====================================================
            var entity = await Query(false) 
                .SingleOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            // =====================================================
            // 2) Bulunmadıysa hata döndür
            // =====================================================
            if (entity is null)
                return Error("Director not found!");

            // =====================================================
            // 3) Eğer bu yönetmenin bağlı Filmleri varsa (FK: Movie.DirectorId)
            // Silinmesine izin VERME.
            // Query<Movie>() → başka entity'de read-only sorgulama için
            // =====================================================
            var hasMovies = Query<Movie>()
                .Any(m => m.DirectorId == entity.Id);

            if (hasMovies)
                return Error("You cannot delete this director because they have movies!");

            // =====================================================
            // 4) Filmi olmayan yönetmeni sil → Delete(entity)
            // Delete() → Remove + SaveChanges()
            // =====================================================
            Delete(entity);

            // =====================================================
            // 5) Başarılı CommandResponse dön
            // Success(message, id)
            // =====================================================
            return Success("Director deleted successfully.", entity.Id);
        }
    }
}
