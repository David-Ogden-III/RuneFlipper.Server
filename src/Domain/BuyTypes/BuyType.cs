using SharedKernel;

namespace Domain.BuyTypes;
public sealed class BuyType : Entity
{
    public int Id { get; set; }
    public string ShortName { get; set; }
    public string Name { get; set; }
}
