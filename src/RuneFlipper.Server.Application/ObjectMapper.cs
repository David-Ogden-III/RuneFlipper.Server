using Microsoft.AspNetCore.Identity;
using RuneFlipper.Server.Application.Roles.TransferObjects;
using RuneFlipper.Server.Application.Trades.Factory;
using RuneFlipper.Server.Application.Trades.TransferObjects;
using RuneFlipper.Server.Domain.Abstractions.TradeFactory;
using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Application;

public class ObjectMapper
{
    private ObjectMapper() { }

    private static ObjectMapper? _instance;

    private static readonly object InstanceLock = new();

    public static ObjectMapper GetInstance()
    {
        if (_instance == null)
        {
            lock (InstanceLock)
            {
                _instance ??= new ObjectMapper();
            }
        }
        return _instance;
    }

    private RuneScapeTradeFactory? _rSFactory;
    private OldSchoolTradeFactory? _oSFactory;

    private RuneScapeTradeFactory RuneScapeTradeFactory
    {
        get
        {
            _rSFactory ??= new RuneScapeTradeFactory();
            return _rSFactory;
        }
    }
    private OldSchoolTradeFactory OldSchoolTradeFactory
    {
        get
        {
            _oSFactory ??= new OldSchoolTradeFactory();
            return _oSFactory;
        }
    }

    public TradeDetails? CreateDetailedTrade(Trade trade)
    {
        var factory = FactorySelector(trade);

        if (factory == null) return null;

        TradeDetails detailedTrade = factory.CreateDetailedTrade(trade);

        return detailedTrade;
    }

    public ICollection<TradeDetails> CreateDetailedTrades(ICollection<Trade> trades)
    {
        List<TradeDetails> detailedTrades = [];

        foreach (Trade trade in trades)
        {
            var detailedTrade = CreateDetailedTrade(trade);

            if (detailedTrade != null) detailedTrades.Add(detailedTrade);
        }
        return detailedTrades;
    }

    public TradeSummary? CreateTradeSummary(Trade trade)
    {
        var factory = FactorySelector(trade);

        if (factory == null) return null;

        TradeSummary tradeSummary = factory.CreateTradeSummary(trade);

        return tradeSummary;
    }

    public ICollection<TradeSummary> CreateTradeSummaries(ICollection<Trade> trades)
    {
        List<TradeSummary> tradeSummaries = [];

        foreach (Trade trade in trades)
        {
            var tradeSummary = CreateTradeSummary(trade);

            if (tradeSummary is not null) tradeSummaries.Add(tradeSummary);
        }

        return tradeSummaries;
    }

    public static Trade CreateNewTrade(NewTrade request)
    {
        Trade newTrade = new()
        {
            Id = Guid.NewGuid().ToString(),
            CharacterId = request.CharacterId,
            ItemId = request.ItemId,
            BuyTypeId = request.BuyTypeId,
            BuyPrice = request.BuyPrice,
            BuyDateTime = request.BuyDateTime,
            SellTypeId = request.SellTypeId,
            SellPrice = request.SellPrice,
            Quantity = request.Quantity,
            SellDateTime = request.SellDateTime,
            IsComplete = request.IsComplete
        };

        return newTrade;
    }

    public static ICollection<Trade> CreateNewTrades(ICollection<NewTrade> requests)
    {
        List<Trade> newTrades = [];

        foreach (NewTrade request in requests)
        {
            Trade newTrade = new()
            {
                Id = Guid.NewGuid().ToString(),
                CharacterId = request.CharacterId,
                ItemId = request.ItemId,
                BuyTypeId = request.BuyTypeId,
                BuyPrice = request.BuyPrice,
                BuyDateTime = request.BuyDateTime,
                SellTypeId = request.SellTypeId,
                SellPrice = request.SellPrice,
                Quantity = request.Quantity,
                SellDateTime = request.SellDateTime,
                IsComplete = request.IsComplete
            };
            newTrades.Add(newTrade);
        }
        return newTrades;
    }

    public static Trade UpdateExistingTrade(Trade existingTrade, UpdateTradeRequest request)
    {
        existingTrade.CharacterId = request.CharacterId;
        existingTrade.ItemId = request.ItemId;
        existingTrade.BuyTypeId = request.BuyTypeId;
        existingTrade.BuyPrice = request.BuyPrice;
        existingTrade.BuyDateTime = request.BuyDateTime;
        existingTrade.SellTypeId = request.SellTypeId;
        existingTrade.SellPrice = request.SellPrice;
        existingTrade.Quantity = request.Quantity;
        existingTrade.SellDateTime = request.SellDateTime;
        existingTrade.IsComplete = request.IsComplete;

        return existingTrade;
    }

    public static ICollection<RoleResponse> CreateRoleResponses(ICollection<IdentityRole> roles)
    {
        List<RoleResponse> responseObjects = [];
        foreach (var role in roles)
        {
            RoleResponse newResponse = new(role.Id, role.Name ?? string.Empty);
            responseObjects.Add(newResponse);
        }

        return responseObjects;
    }

    private ITradeFactory? FactorySelector(Trade trade)
    {
        ITradeFactory? factory;

        switch (trade.Item.ModeId)
        {
            case "OSRS":
                factory = OldSchoolTradeFactory;
                break;
            case "RS":
                factory = RuneScapeTradeFactory;
                break;
            default:
                factory = null;
                break;
        }

        return factory;
    }
}
