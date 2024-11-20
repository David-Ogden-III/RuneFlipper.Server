namespace RuneFlipper.Server.Domain.Abstractions;

public interface IItemResponse
{
    public string Id { get; init; }
    public int InGameId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public bool MembersOnly { get; init; }
    public int TradeLimit { get; init; }
    public string ModeId { get; init; }
}