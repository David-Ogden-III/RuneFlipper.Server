using SharedKernel;

namespace Domain.Characters;
public static class CharacterErrors
{
    public static Error NotFound(Guid characterId) => Error.NotFound(
        "Characters.NotFound",
        $"The character with the Id = '{characterId}' was not found");

    public static Error Unauthorized() => Error.Failure(
        "Characters.Unauthorized",
        "You are not authorized to perform this action.");
}
