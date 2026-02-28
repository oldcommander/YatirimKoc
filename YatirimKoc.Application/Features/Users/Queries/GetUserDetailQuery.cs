using MediatR;
using YatirimKoc.Application.Features.Users.Dtos;

namespace YatirimKoc.Application.Features.Users.Queries;

public class GetUserDetailQuery : IRequest<UserDetailDto>
{
    public Guid UserId { get; set; }
}