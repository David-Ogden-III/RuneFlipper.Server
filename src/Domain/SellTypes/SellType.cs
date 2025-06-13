using Domain.Trades;
using SharedKernel;

namespace Domain.SellTypes;
public sealed class SellType : Entity
{
    public int Id { get; set; }
    public string ShortName { get; set; }
    public string Name { get; set; }
}
