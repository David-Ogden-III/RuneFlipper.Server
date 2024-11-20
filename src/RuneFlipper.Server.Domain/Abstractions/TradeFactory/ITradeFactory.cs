using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Domain.Abstractions.TradeFactory;

public interface ITradeFactory
{
    TradeDetails CreateDetailedTrade(Trade trade);
    TradeSummary CreateTradeSummary(Trade trade);
}
