using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Entities;

namespace Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ModeController(RuneFlipperContext context) : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new(context);

    [HttpGet]
    public async Task<ActionResult<ModeDTO>> Get()
    {
        var modes = await _unitOfWork.ModeRepository.GetListAsync();

        List<ModeDTO> response = [];
        foreach (var mode in modes)
        {
            ModeDTO modeDTO = new(mode.Id, mode.Name);
            response.Add(modeDTO);
        }

        return Ok(response);
    }

    [Authorize(Roles = "Owner, Admin")]
    [HttpPost("CreateMode")]
    public async Task<ActionResult<ModeDTO>> Create([FromBody] ModeDTO newModeDTO)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(newModeDTO.Name) || String.IsNullOrWhiteSpace(newModeDTO.Id)) return BadRequest();

            Mode newMode = new()
            {
                Id = newModeDTO.Id,
                Name = newModeDTO.Name,
            };

            _unitOfWork.ModeRepository.Insert(newMode);

            bool success = await _unitOfWork.SaveAsync() == 1;

            if (success)
            {
                ModeDTO response = new(newMode.Id, newMode.Name);
                return CreatedAtAction(nameof(Create), response);
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    [HttpDelete("{modeId}")]
    [Authorize(Roles = "Owner, Admin")]
    public async Task<ActionResult<ModeDTO>> Delete(string modeId)
    {
        try
        {
            Mode modeToDelete = await _unitOfWork.ModeRepository.GetAsync(filters: [mode => mode.Id == modeId]);

            if (modeToDelete == null) return BadRequest();

            _unitOfWork.ModeRepository.Delete(modeToDelete);

            bool success = await _unitOfWork.SaveAsync() == 1;


            if (success)
            {
                ModeDTO response = new(modeToDelete.Id, modeToDelete.Name);
                return Ok(response);
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }
}
