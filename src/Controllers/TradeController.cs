using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Entities;
using System.Linq.Expressions;
using System.Security.Claims;
using Models;
using Models.TradeFactory;

namespace Controllers;

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


            var item = await _unitOfWork.ItemRepository.GetAsync([item => item.Id == newTrade.ItemId]);
            if (item == null || item.ModeId != character.ModeId) return BadRequest();


            Trade trade = _objectMapper.CreateNewTrade(newTrade);
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

    [HttpPut("{userId}")]
    public async Task<ActionResult<TradeSummary>> Update(string userId, UpdateTradeRequest updateTradeRequest)
    {
        try
        {
            // Checks if character from request exists. Checks if logged-in user owns the character. Checks that userId path is the same as the logged-in user
            var authedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var character = await _unitOfWork.CharacterRepository.GetAsync(filters:
                [character => character.Id == updateTradeRequest.CharacterId]);
            if (character == null || character.UserId != authedUserId || authedUserId != userId) return BadRequest();


            // Checks to make sure that the Characters mode is the same as the item mode
            var item = await _unitOfWork.ItemRepository.GetAsync([item => item.Id == updateTradeRequest.ItemId]);
            if (item == null || item.ModeId != character.ModeId) return BadRequest();


            List<Expression<Func<Trade, bool>>> filters =
            [
                trade => trade.Id == updateTradeRequest.Id,
                trade => trade.CharacterId == updateTradeRequest.CharacterId
            ];
            var originalTrade = await _unitOfWork.TradeRepository.GetAsync(filters);
            if (originalTrade is null || originalTrade.CharacterId != updateTradeRequest.CharacterId) return BadRequest(); // Checks to see if trade exists and is owned by the supplied character

            var newItem = _unitOfWork.ItemRepository.GetAsync([item => item.Id == updateTradeRequest.ItemId]);

            Trade updatedTrade = _objectMapper.UpdateExistingTrade(originalTrade, updateTradeRequest);

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

            List<Expression<Func<Trade, bool>>> filters = [trade => trade.Id == tradeId];
            List<string> tablesToJoin = [nameof(Trade.Character)];

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