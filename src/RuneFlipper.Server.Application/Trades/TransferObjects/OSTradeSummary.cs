using RuneFlipper.Server.Domain.Abstractions.TradeFactory;
using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Application.Trades.TransferObjects;
public class OSTradeSummary(Trade trade) : TradeSummary(trade)
{
    protected override double TaxRate => Constants.OSTaxRate;
    protected override int[] TaxExemptIds => Constants.OSTaxExemptIds;
    protected override long MaxTaxPerItem => Constants.OSMaxTaxPerItem;
}
