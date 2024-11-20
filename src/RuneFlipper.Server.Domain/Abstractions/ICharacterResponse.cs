namespace RuneFlipper.Server.Domain.Abstractions;
public interface ICharacterResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public bool Member { get; init; }
    public string UserId { get; init; }
    public string ModeId { get; init; }
    public DateTime CreatedAt { get; init; }
}
