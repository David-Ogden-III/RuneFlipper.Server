using Models.Entities;

namespace Models.TradeFactory;

public interface IModeFactory
{
    TradeDetails CreateDetailedTrade(Trade trade);
    TradeSummary CreateTradeSummary(Trade trade);
}
