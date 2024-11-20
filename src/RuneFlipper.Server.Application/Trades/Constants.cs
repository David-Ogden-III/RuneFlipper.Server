namespace RuneFlipper.Server.Application.Trades;
public static class Constants
{
    public const double OldSchoolTaxRate = 0.01;
    public const double RuneScapeTaxRate = 0.02;

    public static readonly int[] OldSchoolTaxExemptIds = [13190, 1755, 5325, 1785, 2347, 1733, 233, 5341, 8794, 5329, 5343, 1735, 952, 5331];
    public static readonly int[] RuneScapeTaxExemptIds = [29492];

    public const long OldSchoolMaxTaxPerItem = 5000000;
    public const long RuneScapeMaxTaxPerItem = long.MaxValue;
}