namespace RuneFlipper.Server.Domain.Abstractions.TradeFactory;
public abstract class TradeCalculations
{
    protected abstract double TaxRate { get; }
    protected abstract int[] TaxExemptIds { get; }
    protected abstract long MaxTaxPerItem { get; }

    protected long CalculateTaxPerItem(long grossSalePricePerItem, int itemId)
    {
        if (TaxExemptIds.Contains(itemId)) return 0;

        long taxPerItem = (int)(grossSalePricePerItem * TaxRate);

        if (taxPerItem > MaxTaxPerItem) taxPerItem = MaxTaxPerItem;

        return taxPerItem;
    }

    protected long CalculateTotalSalesTax(long grossSalePricePerItem, int quantity, int itemId)
    {
        long taxPerItem = CalculateTaxPerItem(grossSalePricePerItem, itemId);
        long totalTax = taxPerItem * quantity;
        return totalTax;
    }

    protected long CalculateNetSellPricePerItem(long grossSellPricePerItem, int itemId)
    {
        long taxPerItem = CalculateTaxPerItem(grossSellPricePerItem, itemId);
        long netSellPrice = grossSellPricePerItem - taxPerItem;
        return netSellPrice;
    }

    protected long CalculateGrossBreakEvenPrice(long buyPrice, int itemId)
    {
        if (TaxExemptIds.Contains(itemId)) return buyPrice;

        long breakEvenPrice = (long)Math.Ceiling(buyPrice / (1 - TaxRate));
        return breakEvenPrice;
    }

    protected long CalculateNetProfitPerItem(long grossSellPricePerItem, int itemId, long buyPricePerItem)
    {
        long netSellPrice = CalculateNetSellPricePerItem(grossSellPricePerItem, itemId);
        long profitPerItem = netSellPrice - buyPricePerItem;
        return profitPerItem;
    }

    protected long CalculateTotalNetProfit(long grossSellPricePerItem, int itemId, long buyPricePerItem, int quantity)
    {
        long netProfitPerItem = CalculateNetProfitPerItem(grossSellPricePerItem, itemId, buyPricePerItem);
        long totalNetProfit = netProfitPerItem * quantity;
        return totalNetProfit;
    }
}
