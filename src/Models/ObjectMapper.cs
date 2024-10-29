using Microsoft.AspNetCore.Identity;
using Models.DataTransferObjects;
using Models.Entities;
using Models.TradeFactory;

namespace Models;

public class ObjectMapper
{
    private ObjectMapper() { }

    private static ObjectMapper? _instance;

    private static readonly object _instanceLock = new();

    public static ObjectMapper GetInstance()
    {
        if (_instance == null)
        {
            lock (_instanceLock)
            {
                _instance ??= new ObjectMapper();
            }
        }
        return _instance;
    }

    private RSFactory? _rSFactory;
    private OSFactory? _oSFactory;

    private RSFactory RSFactory
    {
        get
        {
            _rSFactory ??= new RSFactory();
            return _rSFactory;
        }
    }
    private OSFactory OSFactory
    {
        get
        {
            _oSFactory ??= new OSFactory();
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

    public TradeSummary? CreateTradeSummary(Trade trade)
    {
        IModeFactory? factory = FactorySelector(trade);

        if (factory == null) return null;

        TradeSummary tradeSummary = factory.CreateTradeSummary(trade);

        return tradeSummary;
    }

    public Trade CreateNewTrade(NewTrade request)
    {
        Trade newTrade = new()
        {
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

    //public Trade UpdateExistingTrade(Trade existingTrade, UpdateTradeRequest request)
    //{
    //    existingTrade.CharacterId = request.CharacterId;
    //    existingTrade.ItemId = request.ItemId;
    //    existingTrade.BuyTypeId = request.BuyTypeId;
    //    existingTrade.BuyPrice = request.BuyPrice;
    //    existingTrade.BuyDateTime = request.BuyDateTime;
    //    existingTrade.SellTypeId = request.SellTypeId;
    //    existingTrade.SellPrice = request.GrossSellPrice;
    //    existingTrade.Quantity = request.Quantity;
    //    existingTrade.SellDateTime = request.SellDateTime;
    //    existingTrade.IsComplete = request.IsComplete;

    //    return existingTrade;
    //}

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

    private IModeFactory? FactorySelector(Trade trade)
    {
        IModeFactory? factory;

        switch (trade.Item.Mode.Id)
        {
            case "OSRS":
                factory = OSFactory;
                break;
            case "RS":
                factory = RSFactory;
                break;
            default:
                factory = null;
                break;
        }

        return factory;
    }
}
