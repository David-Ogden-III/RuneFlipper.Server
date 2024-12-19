using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuneFlipper.Server.Application.Shared.TransferObjects;
using RuneFlipper.Server.Domain.Entities;
using RuneFlipper.Server.Infrastructure.Persistence;

namespace RuneFlipper.Server.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class BuyTypeController(RuneFlipperContext context) : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new(context);

    [HttpGet]
    public async Task<ActionResult<TransactionType>> Get()
    {
        var buyTypes = await _unitOfWork.BuyTypeRepository.GetListAsync();

        List<TransactionType> response = [];
        foreach (var type in buyTypes)
        {
            TransactionType transactionType = new(type.Id, type.Name);
            response.Add(transactionType);
        }

        return Ok(response);
    }

    [Authorize(Roles = "Owner, Admin")]
    [HttpPost("CreateBuyType")]
    public async Task<ActionResult<TransactionType>> Create([FromBody] TransactionType newBuyTypeDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(newBuyTypeDto.Name) || string.IsNullOrWhiteSpace(newBuyTypeDto.Id)) return BadRequest();

            BuyType newBuyType = new()
            {
                Id = newBuyTypeDto.Id,
                Name = newBuyTypeDto.Name
            };

            _unitOfWork.BuyTypeRepository.Insert(newBuyType);

            bool success = await _unitOfWork.SaveAsync() == 1;

            if (!success) return BadRequest();
            TransactionType response = new(newBuyType.Id, newBuyType.Name);
            return CreatedAtAction(nameof(Create), response);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    [HttpDelete("{buyTypeId}")]
    [Authorize(Roles = "Owner, Admin")]
    public async Task<ActionResult<TransactionType>> Delete(string buyTypeId)
    {
        try
        {
            var buyTypeToDelete = await _unitOfWork.BuyTypeRepository.GetByIdAsync(buyTypeId);

            if (buyTypeToDelete == null) return NotFound();

            _unitOfWork.BuyTypeRepository.Delete(buyTypeToDelete);

            bool success = await _unitOfWork.SaveAsync() == 1;


            if (!success) return BadRequest();

            TransactionType response = new(buyTypeToDelete.Id, buyTypeToDelete.Name);
            return Ok(response);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }
}
