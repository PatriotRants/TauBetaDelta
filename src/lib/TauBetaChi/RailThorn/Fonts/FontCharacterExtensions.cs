using FreeTypeSharp.Native;

namespace ForgeWorks.RailThorn.Fonts;

public static class FontCharacterExtensions
{
    public static (float xpos, float ypos, float w, float h) GetMapping(this Font.Character ch)
    {
        return (ch.Bearing.X, ch.Size.Y - ch.Bearing.Y, ch.Size.X, ch.Size.Y);
    }
}
