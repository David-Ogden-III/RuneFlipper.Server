using RuneFlipper.Server.Domain.Abstractions;

namespace RuneFlipper.Server.Application.Shared.TransferObjects;

public record TransactionType(string Id, string Name) : ITransactionType;
