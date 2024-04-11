namespace ForgeWorks.RailThorn.Fonts;

public interface ICharacterSet : IReadOnlyCollection<Font.Character>
{
    string Name { get; }
    Font.Character this[char c] { get; }
}
