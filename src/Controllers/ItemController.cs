using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ItemController(RuneFlipperContext context) : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new(context);


    [HttpGet("SearchByModeAndName")]
    public async Task<ActionResult<ICollection<ItemResponse>>> Get(string modeId, string itemName)
    {
        string normalizedItemName = itemName.Trim() + "%";

        if (normalizedItemName.Length < 3) return BadRequest("Input must be longer than 3 characters");

        List<Expression<Func<Item, bool>>> filters =
        [
            item => item.ModeId == modeId,
            item => EF.Functions.ILike(item.Name, $"%{normalizedItemName}")
        ];

        Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = queryable => queryable.OrderByDescending(item => EF.Functions.ILike(item.Name, $"{normalizedItemName}"));

        var items = await _unitOfWork.ItemRepository.GetListAsync(filters: filters, orderBy: orderBy, limit: 10);

        List<ItemResponse> response = [];
        foreach (Item item in items)
        {
            ItemResponse itemResponse = new()
            {
                Id = item.Id,
                InGameId = item.InGameId,
                Name = item.Name,
                Description = item.Description,
                MembersOnly = item.MembersOnly,
                TradeLimit = item.TradeLimit,
                ModeId = item.ModeId

            };
            response.Add(itemResponse);
        }

        return Ok(response);
    }

    [HttpGet("FindById/{itemId}")]
    public async Task<ActionResult<ItemResponse>> GetById(string itemId)
    {
        List<Expression<Func<Item, bool>>> filters =
        [
            item => item.Id == itemId
        ];

        var item = await _unitOfWork.ItemRepository.GetAsync(filters: filters);

        if (item is null) return NotFound();

        ItemResponse response = new()
        {
            Id = item.Id,
            InGameId = item.InGameId,
            Name = item.Name,
            Description = item.Description,
            MembersOnly = item.MembersOnly,
            TradeLimit = item.TradeLimit,
            ModeId = item.ModeId

        };

        return Ok(response);
    }

    [Authorize(Roles = "Owner, Admin")]
    [HttpPost("CreateItem")]
    public async Task<ActionResult<ItemResponse>> Create([FromBody] NewItem newItemDto)
    {
        try
        {
            bool newItemIsValid = ValidateDtoProperties(newItemDto);
            if (!newItemIsValid) return BadRequest();

            Item newItem = new()
            {
                Id = Guid.NewGuid().ToString(),
                InGameId = newItemDto.InGameId,
                Name = newItemDto.Name,
                Description = newItemDto.Description,
                MembersOnly = newItemDto.MembersOnly,
                TradeLimit = newItemDto.TradeLimit,
                ModeId = newItemDto.ModeId
            };

            _unitOfWork.ItemRepository.Insert(newItem);

            bool success = await _unitOfWork.SaveAsync() == 1;

            if (!success) return BadRequest();

            ItemResponse response = new()
            {
                Id = newItem.Id,
                InGameId = newItem.InGameId,
                Name = newItem.Name,
                Description = newItem.Description,
                MembersOnly = newItem.MembersOnly,
                TradeLimit = newItem.TradeLimit,
                ModeId = newItem.ModeId
            };
            return CreatedAtAction(nameof(Create), response);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    [Authorize(Roles = "Owner, Admin")]
    [HttpPost("CreateItems")]
    public async Task<ActionResult<ICollection<ItemResponse>>> CreateItems([FromBody] ICollection<NewItem> newItemDTOs)
    {
        try
        {
            List<ItemResponse> items = [];
            foreach (NewItem itemDto in newItemDTOs)
            {
                bool newItemIsValid = ValidateDtoProperties(itemDto);
                if (!newItemIsValid) return BadRequest($"Item is Invalid:\n\t{itemDto.Name}");

                Item newItem = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    InGameId = itemDto.InGameId,
                    Name = itemDto.Name,
                    Description = itemDto.Description,
                    MembersOnly = itemDto.MembersOnly,
                    TradeLimit = itemDto.TradeLimit,
                    ModeId = itemDto.ModeId
                };
                _unitOfWork.ItemRepository.Insert(newItem);

                ItemResponse itemResponse = new()
                {
                    Id = newItem.Id,
                    InGameId = newItem.InGameId,
                    Name = newItem.Name,
                    Description = newItem.Description,
                    MembersOnly = newItem.MembersOnly,
                    TradeLimit = newItem.TradeLimit,
                    ModeId = newItem.ModeId
                };

                items.Add(itemResponse);
            }

            int inputLength = newItemDTOs.Count;
            bool success = await _unitOfWork.SaveAsync() == inputLength;

            if (success)
            {
                return CreatedAtAction(nameof(CreateItems), items);
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    [HttpDelete("{itemId}")]
    [Authorize(Roles = "Owner, Admin")]
    public async Task<ActionResult<ItemResponse>> Delete(string itemId)
    {
        try
        {
            var itemToDelete = await _unitOfWork.ItemRepository.GetAsync(filters: [item => item.Id == itemId]);

            if (itemToDelete == null) return BadRequest();

            _unitOfWork.ItemRepository.Delete(itemToDelete);

            bool success = await _unitOfWork.SaveAsync() == 1;


            if (!success) return BadRequest();

            ItemResponse response = new()
            {
                Id = itemToDelete.Id,
                InGameId = itemToDelete.InGameId,
                Name = itemToDelete.Name,
                Description = itemToDelete.Description,
                MembersOnly = itemToDelete.MembersOnly,
                TradeLimit = itemToDelete.TradeLimit,
                ModeId = itemToDelete.ModeId
            };
            return Ok(response);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    private static bool ValidateDtoProperties(NewItem newItem)
    {
        bool isValid = newItem.InGameId > 0;
        isValid = isValid && !string.IsNullOrWhiteSpace(newItem.Name);
        isValid = isValid && !string.IsNullOrWhiteSpace(newItem.Description);
        isValid = isValid && newItem.TradeLimit >= 0;
        isValid = isValid && !string.IsNullOrWhiteSpace(newItem.ModeId);

        return isValid;
    }
}
