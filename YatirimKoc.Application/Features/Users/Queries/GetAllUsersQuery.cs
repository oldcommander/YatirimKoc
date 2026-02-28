using MediatR;
using YatirimKoc.Application.Features.Users.Dtos;

namespace YatirimKoc.Application.Features.Users.Queries;

public class GetAllUsersQuery : IRequest<List<UserDto>> { }