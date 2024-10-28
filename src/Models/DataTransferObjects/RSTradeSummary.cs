using Models.Entities;
using Models.TradeFactory;

namespace Models.DataTransferObjects;
public class RSTradeSummary(Trade trade) : TradeSummary(trade)
{
    protected override double TaxRate => Constants.RSTaxRate;
    protected override int[] TaxExemptIds => Constants.RSTaxExemptIds;
    protected override long MaxTaxPerItem => Constants.RSMaxTaxPerItem;
}
