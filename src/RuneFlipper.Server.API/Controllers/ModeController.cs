using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuneFlipper.Server.Application.Modes.TransferObjects;
using RuneFlipper.Server.Domain.Entities;
using RuneFlipper.Server.Infrastructure.Persistence;

namespace RuneFlipper.Server.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ModeController(RuneFlipperContext context) : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new(context);

    [HttpGet]
    public async Task<ActionResult<ModeDto>> Get()
    {
        var modes = await _unitOfWork.ModeRepository.GetListAsync();

        List<ModeDto> response = [];
        foreach (var mode in modes)
        {
            ModeDto modeDto = new(mode.Id, mode.Name);
            response.Add(modeDto);
        }

        return Ok(response);
    }

    [Authorize(Roles = "Owner, Admin")]
    [HttpPost("CreateMode")]
    public async Task<ActionResult<ModeDto>> Create([FromBody] ModeDto newModeDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(newModeDto.Name) || string.IsNullOrWhiteSpace(newModeDto.Id)) return BadRequest();

            Mode newMode = new()
            {
                Id = newModeDto.Id,
                Name = newModeDto.Name
            };

            _unitOfWork.ModeRepository.Insert(newMode);

            bool success = await _unitOfWork.SaveAsync() == 1;

            if (!success) return BadRequest();

            ModeDto response = new(newMode.Id, newMode.Name);
            return CreatedAtAction(nameof(Create), response);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    [HttpDelete("{modeId}")]
    [Authorize(Roles = "Owner, Admin")]
    public async Task<ActionResult<ModeDto>> Delete(string modeId)
    {
        try
        {
            var modeToDelete = await _unitOfWork.ModeRepository.GetAsync(filters: [mode => mode.Id == modeId]);

            if (modeToDelete == null) return BadRequest();

            _unitOfWork.ModeRepository.Delete(modeToDelete);

            bool success = await _unitOfWork.SaveAsync() == 1;


            if (!success) return BadRequest();

            ModeDto response = new(modeToDelete.Id, modeToDelete.Name);
            return Ok(response);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }
}
