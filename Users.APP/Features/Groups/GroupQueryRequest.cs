using CORE.APP.Models;
using MediatR;

namespace Users.APP.Features.Groups;

public class GroupQueryRequest : Request, IRequest<IQueryable<GroupQueryResponse>>
{ }