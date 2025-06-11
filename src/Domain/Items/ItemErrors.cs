using SharedKernel;

namespace Domain.Items;
public static class ItemErrors
{
    public static Error NotFound(int itemId) => Error.NotFound(
        "Items.NotFound",
        $"The item with the Id = '{itemId}' was not found");
}
