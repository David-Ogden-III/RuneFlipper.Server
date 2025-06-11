using SharedKernel;

namespace Domain.SellTypes;
public static class SellTypeErrors
{
    public static Error NotFound(int sellTypeId) => Error.NotFound(
        "SellTypes.NotFound",
        $"The sell type with the Id = '{sellTypeId}' was not found");
}
