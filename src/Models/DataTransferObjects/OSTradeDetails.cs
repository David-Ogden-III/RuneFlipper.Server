using Models.Entities;
using Models.TradeFactory;

namespace Models.DataTransferObjects;
public class OSTradeDetails(Trade trade) : TradeDetails(trade)
{
    protected override double TaxRate => Constants.OSTaxRate;
    protected override int[] TaxExemptIds => Constants.OSTaxExemptIds;
    protected override long MaxTaxPerItem => Constants.OSMaxTaxPerItem;
}
