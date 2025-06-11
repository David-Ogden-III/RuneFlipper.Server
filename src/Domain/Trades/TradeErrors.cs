using SharedKernel;

namespace Domain.Trades;
public static class TradeErrors
{
    public static Error NotFound(Guid tradeId) => Error.NotFound(
        "Trades.NotFound",
        $"The trade with the Id = '{tradeId}' was not found");

    public static Error Unauthorized() => Error.Failure(
        "Trades.Unauthorized",
        "You are not authorized to perform this action.");
}
