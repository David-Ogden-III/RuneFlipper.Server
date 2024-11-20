using RuneFlipper.Server.Application.Characters.TransferObjects;
using RuneFlipper.Server.Application.Items.TransferObjects;
using RuneFlipper.Server.Application.Shared.TransferObjects;
using RuneFlipper.Server.Domain.Abstractions;
using RuneFlipper.Server.Domain.Abstractions.TradeFactory;
using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Application.Trades.TransferObjects;
public class RSTradeDetails(Trade trade) : TradeDetails(trade)
{
    protected override double TaxRate => Constants.RSTaxRate;
    protected override int[] TaxExemptIds => Constants.RSTaxExemptIds;
    protected override long MaxTaxPerItem => Constants.RSMaxTaxPerItem;


    public override IItemResponse Item { get; set; } = new ItemResponse()
    {
        Id = trade.ItemId,
        InGameId = trade.Item.InGameId,
        Name = trade.Item.Name,
        Description = trade.Item.Description,
        MembersOnly = trade.Item.MembersOnly,
        TradeLimit = trade.Item.TradeLimit,
        ModeId = trade.Item.ModeId
    };

    public override ICharacterResponse Character { get; set; } = new CharacterResponse()
    {
        Id = trade.CharacterId,
        Name = trade.Character.Name,
        Member = trade.Character.Member,
        UserId = trade.Character.UserId,
        CreatedAt = trade.Character.CreatedAt,
        ModeId = trade.Character.ModeId
    };

    public override ITransactionType BuyType { get; set; } = new TransactionType(trade.BuyTypeId, trade.BuyType.Name);
    public override ITransactionType SellType { get; set; } = new TransactionType(trade.SellTypeId, trade.SellType.Name);
}
