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


    [HttpGet]
    public async Task<ActionResult<ItemResponse>> Get(string? identifier, string modeId)
    {
        List<Expression<Func<Item, bool>>> filters = [];
        filters.Add(item => item.ModeId == modeId);
        if (!String.IsNullOrWhiteSpace(identifier))
        {
            filters.Add(item => item.Id == identifier || item.Name.Contains(identifier, StringComparison.CurrentCultureIgnoreCase));
        }


        var items = await _unitOfWork.ItemRepository.GetListAsync(filters: filters);

        List<ItemResponse> response = [];
        foreach (Item item in items)
        {
            ItemResponse itemResponse = new()
            {
                Id = item.Id,
                InGameId = item.InGameId,
                Name = item.Name,
                Description = item.Description,
                MembersOnly = item.Member,
                TradeLimit = item.TradeLimit,
                ModeId = item.ModeId

            };
            response.Add(itemResponse);
        }

        return Ok(response);
    }
}
