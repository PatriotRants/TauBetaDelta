using ForgeWorks.RailThorn.Fonts.Native;

namespace ForgeWorks.RailThorn.Fonts;

public class TrueTypeFont : Font
{
    public TrueTypeFont(Face face) : base(face)
    {
        Name = Path.GetFileNameWithoutExtension(face.FontFile);
    }

    protected override CharacterSet MapFont()
    {
        if (!IsLoaded)
        {
            using (var mapper = new ChartacterTexMap(Face))
            {
                characterSet = new(Name, mapper.MapFont());
            }
        }

        return characterSet;
    }
}


//  Not being used
public class BitmapFont : Font
{
    public BitmapFont() : base(null)
    {
        Name = "Default";
    }

    protected override CharacterSet MapFont()
    {
        throw new NotImplementedException();
    }
}
