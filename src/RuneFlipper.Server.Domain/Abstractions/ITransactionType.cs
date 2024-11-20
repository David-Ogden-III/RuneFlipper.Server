namespace RuneFlipper.Server.Domain.Abstractions;
public interface ITransactionType
{
    public string Id { get; init; }
    public string Name { get; init; }
}