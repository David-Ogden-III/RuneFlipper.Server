using RuneFlipper.Server.Application.Trades.TransferObjects;
using RuneFlipper.Server.Domain.Abstractions.TradeFactory;
using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Application.Trades.Factory;
public class RSFactory : IModeFactory
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