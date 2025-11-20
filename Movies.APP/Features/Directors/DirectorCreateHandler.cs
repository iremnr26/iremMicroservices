using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Directors
{
    public class DirectorCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        public bool IsRetired { get; set; }
    }

    public class DirectorCreateHandler : Service<Director>,
        IRequestHandler<DirectorCreateRequest, CommandResponse>
    {
        public DirectorCreateHandler(MoviesDb db) : base(db) {}

        public async Task<CommandResponse> Handle(DirectorCreateRequest request,
            CancellationToken cancellationToken)
        {
            // Aynı FirstName + LastName combination var mı?
            if (await Query()
                    .AnyAsync(d =>
                            d.FirstName == request.FirstName.Trim()
                            && d.LastName == request.LastName.Trim(),
                        cancellationToken))
            {
                return Error("This director already exists!");
            }

            var entity = new Director
            {
                FirstName = request.FirstName.Trim(),
                LastName  = request.LastName.Trim(),
                IsRetired = request.IsRetired
            };

            Create(entity);

            return Success("Director created successfully.", entity.Id);
        }
    }
}