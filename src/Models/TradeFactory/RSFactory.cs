using Models.DataTransferObjects;
using Models.Entities;

namespace Models.TradeFactory;
public class RSFactory() : IModeFactory
{
    public TradeDetails CreateDetailedTrade(Trade trade)
    {
        return new RSTradeDetails(trade);
    }

    public TradeSummary CreateTradeSummary(Trade trade)
    {
        return new RSTradeSummary(trade);
    }
}