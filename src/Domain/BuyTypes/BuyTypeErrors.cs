using SharedKernel;

namespace Domain.BuyTypes;
public static class BuyTypeErrors
{
    public static Error NotFound(int buyTypeId) => Error.NotFound(
        "BuyTypes.NotFound",
        $"The buy type with the Id = '{buyTypeId}' was not found");
}
