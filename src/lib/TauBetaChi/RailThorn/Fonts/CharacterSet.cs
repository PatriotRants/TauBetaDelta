using System.Collections;
using OpenTK.Mathematics;

namespace ForgeWorks.RailThorn.Fonts;

public class CharacterSet : ICharacterSet
{
    private readonly Dictionary<char, Font.Character> set;

    public string Name { get; }
    public Font.Character this[char c]
    {
        get => set[c];
    }
    public int Count => set.Count;

    public CharacterSet(string name, Font.Character[] characters)
    {
        Name = name;
        set = characters
            .ToDictionary(k => k.Glyph, v => v);
    }

    public IEnumerator<Font.Character> GetEnumerator()
    {
        return set
            .OfType<Font.Character>()
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public interface ICharacterSet : IReadOnlyCollection<Font.Character>
{
    string Name { get; }
    Font.Character this[char c] { get; }
}

public static class FontCharacterExtensions
{
    public static (float xpos, float ypos, float w, float h) GetMapping(this Font.Character ch)
    {
        return (ch.Bearing.X, ch.Size.Y - ch.Bearing.Y, ch.Size.X, ch.Size.Y);
    }
}
