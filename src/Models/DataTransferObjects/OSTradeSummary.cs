using Models.Entities;
using Models.TradeFactory;

namespace Models.DataTransferObjects;
public class OSTradeSummary(Trade trade) : TradeSummary(trade)
{
    protected override double TaxRate { get; } = Constants.OSTaxRate;
    protected override int[] TaxExemptIds { get; } = Constants.OSTaxExemptIds;
    protected override long MaxTaxPerItem { get; } = Constants.OSMaxTaxPerItem;
}
