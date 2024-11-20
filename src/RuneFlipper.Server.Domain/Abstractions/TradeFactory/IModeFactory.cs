using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Domain.Abstractions.TradeFactory;

public interface IModeFactory
{
    TradeDetails CreateDetailedTrade(Trade trade);
    TradeSummary CreateTradeSummary(Trade trade);
}
