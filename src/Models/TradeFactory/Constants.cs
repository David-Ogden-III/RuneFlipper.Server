namespace Models.TradeFactory;
public static class Constants
{
    public const double OSTaxRate = 0.01;
    public const double RSTaxRate = 0.02;

    public static readonly int[] OSTaxExemptIds = [13190, 1755, 5325, 1785, 2347, 1733, 233, 5341, 8794, 5329, 5343, 1735, 952, 5331];
    public static readonly int[] RSTaxExemptIds = [29492];

    public const long OSMaxTaxPerItem = 5000000;
    public const long RSMaxTaxPerItem = long.MaxValue;
}