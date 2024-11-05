using Models.Entities;
using Models.TradeFactory;

namespace Models.DataTransferObjects;
public class OSTradeSummary(Trade trade) : TradeSummary(trade)
{
    protected override double TaxRate => Constants.OSTaxRate;
    protected override int[] TaxExemptIds => Constants.OSTaxExemptIds;
    protected override long MaxTaxPerItem => Constants.OSMaxTaxPerItem;
}
