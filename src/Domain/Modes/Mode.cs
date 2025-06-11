using Domain.Characters;
using Domain.Items;
using SharedKernel;

namespace Domain.Modes;
public sealed class Mode : Entity
{
    public int Id { get; set; }
    public string ShortName { get; set; }
    public string Name { get; set; }

    public ICollection<Character> Characters { get; set; } = [];
    public ICollection<Item> Items { get; set; } = [];
}
