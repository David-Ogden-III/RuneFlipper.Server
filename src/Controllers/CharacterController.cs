using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Entities;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class CharacterController(RuneFlipperContext context) : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new(context);

    [HttpGet("{userId}")]
    public async Task<ActionResult<ICollection<CharacterResponse>>> Get(string userId, string? modeId, bool? member)
    {
        var authedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (authedUserId != userId) return Forbid();

        List<Expression<Func<Character, bool>>> filters =
        [
            character => character.UserId == userId
        ];

        if (!string.IsNullOrWhiteSpace(modeId)) filters.Add(character => character.ModeId == modeId);
        if (member != null) filters.Add(character => character.Member == member);


        var characters = await _unitOfWork.CharacterRepository.GetListAsync(filters: filters);

        List<CharacterResponse> response = [];
        foreach (var character in characters)
        {
            CharacterResponse characterDto = new()
            {
                Id = character.Id,
                Name = character.Name,
                Member = character.Member,
                UserId = character.UserId,
                CreatedAt = character.CreatedAt,
                ModeId = character.ModeId
            };
            response.Add(characterDto);
        }

        return Ok(response);
    }

    [HttpGet("{userId}/{characterId}")]
    public async Task<ActionResult<CharacterResponse>> GetCharacter(string userId, string characterId)
    {
        var authedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (authedUserId != userId) return Forbid();

        List<Expression<Func<Character, bool>>> filters =
        [
            character => character.UserId == userId,
            character => character.Id == characterId
        ];

        var character = await _unitOfWork.CharacterRepository.GetAsync(filters: filters);

        if (character == null) return NotFound();

        CharacterResponse response = new()
        {
            Id = character.Id,
            Name = character.Name,
            Member = character.Member,
            UserId = character.UserId,
            CreatedAt = character.CreatedAt,
            ModeId = character.ModeId
        };

        return Ok(response);
    }


    [HttpPost]
    public async Task<ActionResult<CharacterResponse>> Create([FromBody] NewCharacter newCharacterDto)
    {
        try
        {
            var authedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authedUserId != newCharacterDto.UserId) return Forbid();

            if (!InputIsValid(newCharacterDto)) return BadRequest();

            Character newCharacter = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = newCharacterDto.Name,
                ModeId = newCharacterDto.ModeId,
                UserId = newCharacterDto.UserId,
                CreatedAt = newCharacterDto.CreatedAt,
                Member = newCharacterDto.Member

            };

            _unitOfWork.CharacterRepository.Insert(newCharacter);

            bool success = await _unitOfWork.SaveAsync() == 1;

            if (!success) return BadRequest();

            CharacterResponse response = new()
            {
                Id = newCharacter.Id,
                Name = newCharacter.Name,
                Member = newCharacter.Member,
                UserId = newCharacter.UserId,
                CreatedAt = newCharacter.CreatedAt,
                ModeId = newCharacter.ModeId
            };
            return CreatedAtAction(nameof(Create), response);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    [HttpDelete("{userId}/{characterId}")]
    public async Task<ActionResult<CharacterResponse>> Delete(string userId, string characterId)
    {
        try
        {
            var authedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authedUserId != userId) return Forbid();

            List<Expression<Func<Character, bool>>> filters =
            [
                character => character.UserId == userId,
                character => character.Id == characterId
            ];

            var characterToDelete = await _unitOfWork.CharacterRepository.GetAsync(filters: filters);

            if (characterToDelete == null) return BadRequest();

            _unitOfWork.CharacterRepository.Delete(characterToDelete);

            bool success = await _unitOfWork.SaveAsync() == 1;


            if (!success) return BadRequest();

            CharacterResponse response = new()
            {
                Id = characterToDelete.Id,
                Name = characterToDelete.Name,
                Member = characterToDelete.Member,
                UserId = characterToDelete.UserId,
                CreatedAt = characterToDelete.CreatedAt,
                ModeId = characterToDelete.ModeId
            };
            return Ok(response);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    private static bool InputIsValid(NewCharacter character)
    {
        bool isValid = !string.IsNullOrWhiteSpace(character.Name);
        isValid = !string.IsNullOrWhiteSpace(character.ModeId) && isValid;
        isValid = !string.IsNullOrWhiteSpace(character.UserId) && isValid;
        return isValid;
    }
}

