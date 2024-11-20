using RuneFlipper.Server.Domain.Abstractions;

namespace RuneFlipper.Server.Application.Characters.TransferObjects;
public record CharacterResponse : ICharacterResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required bool Member { get; init; }
    public required string UserId { get; init; }
    public required string ModeId { get; init; }
    public required DateTime CreatedAt { get; init; }
}
