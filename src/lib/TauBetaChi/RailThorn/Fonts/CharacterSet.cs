using System.Collections;

namespace ForgeWorks.RailThorn.Fonts;

public class CharacterSet : ICharacterSet
{
    private readonly Dictionary<uint, Font.Character> set;

    public string Name { get; }
    public Font.Character this[char c] => set[c];
    public int Count => set.Count;

    public CharacterSet(string name, Font.Character[] characters)
    {
        Name = name;
        set = characters
            .ToDictionary(k => (uint)k.Glyph, v => v);
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
