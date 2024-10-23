using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Entities;

namespace Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class SellTypeController(RuneFlipperContext context) : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new(context);

    [HttpGet]
    public async Task<ActionResult<TransactionType>> Get()
    {
        var sellTypes = await _unitOfWork.SellTypeRepository.GetListAsync();

        List<TransactionType> response = [];
        foreach (var type in sellTypes)
        {
            TransactionType transactionType = new(type.Id, type.Name);
            response.Add(transactionType);
        }

        return Ok(response);
    }

    [Authorize(Roles = "Owner, Admin")]
    [HttpPost("CreateSellType")]
    public async Task<ActionResult<TransactionType>> Create([FromBody] TransactionType newSellTypeDTO)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(newSellTypeDTO.Name) || String.IsNullOrWhiteSpace(newSellTypeDTO.Id)) return BadRequest();

            SellType newSellType = new()
            {
                Id = newSellTypeDTO.Id,
                Name = newSellTypeDTO.Name,
            };

            _unitOfWork.SellTypeRepository.Insert(newSellType);

            bool success = await _unitOfWork.SaveAsync() == 1;

            if (success)
            {
                TransactionType response = new(newSellType.Id, newSellType.Name);
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

    [HttpDelete("{sellTypeId}")]
    [Authorize(Roles = "Owner, Admin")]
    public async Task<ActionResult<TransactionType>> Delete(string sellTypeId)
    {
        try
        {
            SellType sellTypeToDelete = await _unitOfWork.SellTypeRepository.GetAsync(filters: [sellType => sellType.Id == sellTypeId]);

            if (sellTypeToDelete == null) return BadRequest();

            _unitOfWork.SellTypeRepository.Delete(sellTypeToDelete);

            bool success = await _unitOfWork.SaveAsync() == 1;


            if (success)
            {
                TransactionType response = new(sellTypeToDelete.Id, sellTypeToDelete.Name);
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
