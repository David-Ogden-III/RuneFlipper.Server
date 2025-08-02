namespace Domain.Items;
public sealed class Item
{
    public int Id { get; set; }
    public int InGameId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsMembers { get; set; }
    public int TradeLimit { get; set; }
    public int ModeId { get; set; }
}
