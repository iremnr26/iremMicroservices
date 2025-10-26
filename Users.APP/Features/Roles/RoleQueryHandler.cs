using CORE.APP.Models;
using MediatR;

namespace Users.APP.Features.Roles;

public class RoleQueryRequest : Request, IRequest<IQueryable<RoleQueryResponse>>;

public class RoleQueryResponse : Response;

public class RoleQueryHandler{}
