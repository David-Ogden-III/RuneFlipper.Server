using SharedKernel;

namespace Domain.Modes;
public static class ModeErrors
{
    public static Error NotFound(int modeId) => Error.NotFound(
        "Modes.NotFound",
        $"The mode with the Id = '{modeId}' was not found");
}
