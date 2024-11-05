namespace Models.DataTransferObjects;
public record CharacterResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required bool Member { get; init; }
    public required string UserId { get; init; }
    public required string ModeId { get; init; }
    public required DateTime CreatedAt { get; init; }
}
