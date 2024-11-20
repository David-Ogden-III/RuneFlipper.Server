using RuneFlipper.Server.Domain.Abstractions.TradeFactory;
using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Application.Trades.TransferObjects;
public class RSTradeSummary(Trade trade) : TradeSummary(trade)
{
    protected override double TaxRate => Constants.RSTaxRate;
    protected override int[] TaxExemptIds => Constants.RSTaxExemptIds;
    protected override long MaxTaxPerItem => Constants.RSMaxTaxPerItem;
}
