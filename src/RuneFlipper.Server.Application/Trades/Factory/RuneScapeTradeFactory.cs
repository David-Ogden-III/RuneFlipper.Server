using RuneFlipper.Server.Application.Trades.TransferObjects;
using RuneFlipper.Server.Domain.Abstractions.TradeFactory;
using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Application.Trades.Factory;
public class RuneScapeTradeFactory : ITradeFactory
{
    public TradeDetails CreateDetailedTrade(Trade trade)
    {
        return new RuneScapeTradeDetails(trade);
    }

    public TradeSummary CreateTradeSummary(Trade trade)
    {
        return new RuneScapeTradeSummary(trade);
    }
}