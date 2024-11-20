using RuneFlipper.Server.Domain.Abstractions.TradeFactory;
using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Application.Trades.TransferObjects;
public class OldSchoolTradeSummary(Trade trade) : TradeSummary(trade)
{
    protected override double TaxRate => Constants.OldSchoolTaxRate;
    protected override int[] TaxExemptIds => Constants.OldSchoolTaxExemptIds;
    protected override long MaxTaxPerItem => Constants.OldSchoolMaxTaxPerItem;
}
