using RuneFlipper.Server.Domain.Abstractions.TradeFactory;
using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Application.Trades.TransferObjects;
public class RuneScapeTradeSummary(Trade trade) : TradeSummary(trade)
{
    protected override double TaxRate => Constants.RuneScapeTaxRate;
    protected override int[] TaxExemptIds => Constants.RuneScapeTaxExemptIds;
    protected override long MaxTaxPerItem => Constants.RuneScapeMaxTaxPerItem;
}
