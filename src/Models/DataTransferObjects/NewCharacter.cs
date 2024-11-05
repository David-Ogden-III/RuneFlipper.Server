namespace Models.DataTransferObjects;
public record NewCharacter
{
    public required string Name { get; init; }
    public required string ModeId { get; init; }
    public required string UserId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required bool Member { get; init; }

}

