﻿using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Entities;

namespace Controllers;

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
    public async Task<ActionResult<TransactionType>> Create([FromBody] TransactionType newBuyTypeDTO)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(newBuyTypeDTO.Name) || String.IsNullOrWhiteSpace(newBuyTypeDTO.Id)) return BadRequest();

            BuyType newBuyType = new()
            {
                Id = newBuyTypeDTO.Id,
                Name = newBuyTypeDTO.Name,
            };

            _unitOfWork.BuyTypeRepository.Insert(newBuyType);

            bool success = await _unitOfWork.SaveAsync() == 1;

            if (success)
            {
                TransactionType response = new(newBuyType.Id, newBuyType.Name);
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

    [HttpDelete("{buyTypeId}")]
    [Authorize(Roles = "Owner, Admin")]
    public async Task<ActionResult<TransactionType>> Delete(string buyTypeId)
    {
        try
        {
            BuyType buyTypeToDelete = await _unitOfWork.BuyTypeRepository.GetAsync(filters: [buyType => buyType.Id == buyTypeId]);

            if (buyTypeToDelete == null) return BadRequest();

            _unitOfWork.BuyTypeRepository.Delete(buyTypeToDelete);

            bool success = await _unitOfWork.SaveAsync() == 1;


            if (success)
            {
                TransactionType response = new(buyTypeToDelete.Id, buyTypeToDelete.Name);
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