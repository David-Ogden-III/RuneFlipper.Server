using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Entities;
using System.Linq.Expressions;

namespace Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ItemController(RuneFlipperContext context) : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new(context);


    [HttpGet("{modeId}")]
    public async Task<ActionResult<ICollection<ItemResponse>>> Get(string modeId, string? itemId, string? itemName)
    {
        List<Expression<Func<Item, bool>>> filters = [];
        filters.Add(item => item.ModeId == modeId);

        bool idIsNull = String.IsNullOrWhiteSpace(itemId);

        if (!idIsNull)
        {
            filters.Add(item => item.Id == itemId);
        }


        ICollection<Item> items = await _unitOfWork.ItemRepository.GetListAsync(filters: filters);

        if (!String.IsNullOrWhiteSpace(itemName) && idIsNull)
        {
            items = items.Where(item => item.Name.Contains(itemName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

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

    [Authorize(Roles = "Owner, Admin")]
    [HttpPost("CreateItem")]
    public async Task<ActionResult<ItemResponse>> Create([FromBody] NewItem newItemDTO)
    {
        try
        {
            bool newItemIsValid = ValidateDTOProperties(newItemDTO);
            if (!newItemIsValid) return BadRequest();

            Item newItem = new()
            {
                Id = Guid.NewGuid().ToString(),
                InGameId = newItemDTO.InGameId,
                Name = newItemDTO.Name,
                Description = newItemDTO.Description,
                MembersOnly = newItemDTO.MembersOnly,
                TradeLimit = newItemDTO.TradeLimit,
                ModeId = newItemDTO.ModeId
            };

            _unitOfWork.ItemRepository.Insert(newItem);

            bool success = await _unitOfWork.SaveAsync() == 1;

            if (success)
            {
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

            return BadRequest();
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
            foreach (NewItem itemDTO in newItemDTOs)
            {
                bool newItemIsValid = ValidateDTOProperties(itemDTO);
                if (!newItemIsValid) return BadRequest($"Item is Invalid:\n\t{itemDTO.Name}");

                Item newItem = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    InGameId = itemDTO.InGameId,
                    Name = itemDTO.Name,
                    Description = itemDTO.Description,
                    MembersOnly = itemDTO.MembersOnly,
                    TradeLimit = itemDTO.TradeLimit,
                    ModeId = itemDTO.ModeId
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
            Item itemToDelete = await _unitOfWork.ItemRepository.GetAsync(filters: [item => item.Id == itemId]);

            if (itemToDelete == null) return BadRequest();

            _unitOfWork.ItemRepository.Delete(itemToDelete);

            bool success = await _unitOfWork.SaveAsync() == 1;


            if (success)
            {
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

            return BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    private static bool ValidateDTOProperties(NewItem newItem)
    {
        bool isValid = newItem.InGameId > 0;
        isValid = isValid && !String.IsNullOrWhiteSpace(newItem.Name);
        isValid = isValid && !String.IsNullOrWhiteSpace(newItem.Description);
        isValid = isValid && newItem.TradeLimit > 0;
        isValid = isValid && !String.IsNullOrWhiteSpace(newItem.ModeId);

        return isValid;
    }
}
