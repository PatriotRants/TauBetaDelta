using System.Collections;

namespace ForgeWorks.RailThorn.Fonts;

public class CharacterSet : IReadOnlyCollection<Font.Character>
{
    private Font.Character[] set;

    public int Count => set.Length;

    public CharacterSet(Font.Character[] characters)
    {
        set = characters;
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
