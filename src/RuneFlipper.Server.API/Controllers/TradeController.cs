using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuneFlipper.Server.Application;
using RuneFlipper.Server.Application.Trades.TransferObjects;
using RuneFlipper.Server.Domain.Abstractions.TradeFactory;
using RuneFlipper.Server.Domain.Entities;
using RuneFlipper.Server.Infrastructure.Persistence;
using System.Linq.Expressions;
using System.Security.Claims;

namespace RuneFlipper.Server.API.Controllers;

[Route("[controller]"), ApiController, Authorize]
public class TradeController(RuneFlipperContext context) : ControllerBase
{
    private readonly ObjectMapper _objectMapper = ObjectMapper.GetInstance();
    private readonly UnitOfWork _unitOfWork = new(context);



    [HttpGet("DetailedTrade/{userId}/{tradeId}")]
    public async Task<ActionResult<TradeDetails>> Get(string userId, string tradeId)
    {
        var authedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (authedUserId != userId) return Forbid();

        List<Expression<Func<Trade, bool>>> filters =
        [
            trade => trade.Character.UserId == userId,
            trade => trade.Id == tradeId
        ];

        List<string> tablesToJoin =
            [nameof(Trade.Character), nameof(Trade.BuyType), nameof(Trade.SellType), nameof(Trade.Item)];

        var trade = await _unitOfWork.TradeRepository.GetAsync(filters: filters, tablesToJoin: tablesToJoin);

        if (trade is null) return NotFound();

        TradeDetails? response = _objectMapper.CreateDetailedTrade(trade);

        return Ok(response);
    }

    [HttpGet("TradeSummaries/{userId}")]
    public async Task<ActionResult<ICollection<TradeSummary>>> GetList(string userId, string? modeId, bool? member, string? characterId)
    {
        var authedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (authedUserId != userId) return Forbid();

        List<Expression<Func<Trade, bool>>> filters =
        [
            trade => trade.Character.UserId == userId
        ];

        bool characterIdNotNull = !string.IsNullOrWhiteSpace(characterId);
        bool modeIdNotNull = !string.IsNullOrWhiteSpace(modeId);
        if (characterIdNotNull) filters.Add(trade => trade.CharacterId == characterId);
        if (modeIdNotNull && !characterIdNotNull) filters.Add(trade => trade.Character.ModeId == modeId);
        if (member != null) filters.Add(trade => trade.Item.MembersOnly == member);

        List<string> tablesToJoin =
            [nameof(Trade.Character), nameof(Trade.BuyType), nameof(Trade.SellType), nameof(Trade.Item)];

        var trades = await _unitOfWork.TradeRepository.GetListAsync(filters: filters, tablesToJoin: tablesToJoin);

        List<TradeSummary> response = _objectMapper.CreateTradeSummaries(trades).ToList();

        return Ok(response);
    }

    [HttpPost("CreateTrade")]
    public async Task<ActionResult> Create(NewTrade newTrade)
    {
        try
        {
            var authedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var character = await _unitOfWork.CharacterRepository.GetAsync(filters:
                [character => character.Id == newTrade.CharacterId]);
            if (character == null || character.UserId != authedUserId) return BadRequest();

            if (newTrade.BuyDateTime > newTrade.SellDateTime || newTrade.Quantity <= 0) return BadRequest();

            var item = await _unitOfWork.ItemRepository.GetAsync([item => item.Id == newTrade.ItemId]);
            if (item == null || item.ModeId != character.ModeId) return BadRequest();


            Trade trade = ObjectMapper.CreateNewTrade(newTrade);
            _unitOfWork.TradeRepository.Insert(trade);
            bool success = await _unitOfWork.SaveAsync() == 1;

            if (!success) return BadRequest();


            return Created();
        }
        catch
        {
            return BadRequest();
        }
    }


    [HttpPost("CreateTrades"), Authorize(Roles = "Owner, Admin")]
    public async Task<ActionResult> CreateTrades(ICollection<NewTrade> newTrades)
    {
        try
        {
            var trades = ObjectMapper.CreateNewTrades(newTrades);

            foreach (Trade trade in trades)
            {
                _unitOfWork.TradeRepository.Insert(trade);
            }

            await _unitOfWork.SaveAsync();

            return Created();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut("EditTrade")]
    public async Task<ActionResult<TradeSummary>> Update(UpdateTradeRequest updateTradeRequest)
    {
        try
        {
            var authedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Make sure trade exists and authed user owns the character that owns the trade
            Expression<Func<Trade, bool>>[] filters = [trade => trade.Id == updateTradeRequest.Id];
            string[] tablesToJoin =
                [nameof(Trade.Character), nameof(Trade.BuyType), nameof(Trade.SellType), nameof(Trade.Item)];
            var originalTrade = await _unitOfWork.TradeRepository.GetAsync(filters, tablesToJoin);
            if (originalTrade is null) return NotFound("Trade Not Found");
            if (originalTrade.Character.UserId != authedUserId) return Forbid();

            if (updateTradeRequest.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            if (updateTradeRequest.BuyDateTime > updateTradeRequest.SellDateTime)
            {
                return BadRequest("Purchase time must be before sale time");
            }


            // If character or Item changes:
            // make sure new character/item exists, authed user owns the new character, and character mode == item mode
            if (originalTrade.CharacterId != updateTradeRequest.CharacterId || originalTrade.ItemId != updateTradeRequest.ItemId)
            {
                var character = await _unitOfWork.CharacterRepository.GetAsync(filters:
                    [character => character.Id == updateTradeRequest.CharacterId]);
                if (character == null) return NotFound("Character Not Found");
                if (character.UserId != authedUserId) return Forbid();

                var item = await _unitOfWork.ItemRepository.GetAsync([item => item.Id == updateTradeRequest.ItemId]);
                if (item == null) return NotFound("Item Not Found");
                if (item.ModeId != character.ModeId) return BadRequest("Item mode does not match character mode");
            }

            Trade updatedTrade = ObjectMapper.UpdateExistingTrade(originalTrade, updateTradeRequest);

            _unitOfWork.TradeRepository.Update(updatedTrade);

            bool success = await _unitOfWork.SaveAsync() == 1;

            if (!success) return BadRequest();

            var response = _objectMapper.CreateTradeSummary(updatedTrade);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }

    [HttpDelete("{userId}/{tradeId}")]
    public async Task<ActionResult<TradeSummary>> Delete(string userId, string tradeId)
    {
        try
        {
            var authedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authedUserId != userId) return Forbid();

            List<Expression<Func<Trade, bool>>> filters = [trade => trade.Id == tradeId, trade => trade.Character.UserId == userId];
            List<string> tablesToJoin =
                [nameof(Trade.Character), nameof(Trade.BuyType), nameof(Trade.SellType), nameof(Trade.Item)];

            var trade = await _unitOfWork.TradeRepository.GetAsync(filters, tablesToJoin);

            if (trade is null || trade.Character.UserId != userId) return BadRequest();

            _unitOfWork.TradeRepository.Delete(trade);
            bool success = await _unitOfWork.SaveAsync() == 1;

            if (!success) return BadRequest();

            var response = _objectMapper.CreateTradeSummary(trade);

            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }
}