using Models.DataTransferObjects;
using Models.Entities;

namespace Models.TradeFactory;
public class OSFactory() : IModeFactory
{
    public TradeDetails CreateDetailedTrade(Trade trade)
    {
        return new OSTradeDetails(trade);
    }

    public TradeSummary CreateTradeSummary(Trade trade)
    {
        return new OSTradeSummary(trade);
    }
}