using RuneFlipper.Server.Application.Trades.TransferObjects;
using RuneFlipper.Server.Domain.Abstractions.TradeFactory;
using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Application.Trades.Factory;
public class OSFactory : IModeFactory
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