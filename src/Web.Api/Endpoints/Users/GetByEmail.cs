using Application.Abstractions.Messaging;
using Application.Users.GetByEmail;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class GetByEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("user/{userEmail}", async (
            string userEmail,
            IQueryHandler<GetUserByEmailQuery, UserResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserByEmailQuery(userEmail);

            Result<UserResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.UsersAccess)
        .WithTags(Tags.Users);
    }
}
