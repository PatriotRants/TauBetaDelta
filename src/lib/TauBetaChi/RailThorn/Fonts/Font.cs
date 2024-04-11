using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using ForgeWorks.RailThorn.Fonts.Native;

namespace ForgeWorks.RailThorn.Fonts;

public abstract class Font
{
    public static Font Default => FontManager.Instance.Default;

    private readonly Lazy<CharacterSet> _characterSet;
    private readonly Face face;


    protected CharacterSet characterSet;
    protected FontLibrary FontLib => face.FontLib;
    protected Face Face => face;

    public ICharacterSet CharacterSet => _characterSet.Value;
    public bool IsLoaded => characterSet?.Count > 0;
    public string Name { get; protected init; }
    public uint Height
    {
        get => face.PixelHeight;
        set => face.SetPixelSize(0, value);
    }

    public Font(Face fontFace)
    {
        face = fontFace;
        _characterSet = new(MapFont);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fontName"></param>
    public static implicit operator Font(string fontName)
    {
        return FontManager.Instance.GetFont(fontName);
    }

    /// <summary>
    /// lazy loads the character set
    /// </summary>
    protected abstract CharacterSet MapFont();


    public struct Character
    {
        public char Glyph { get; set; }
        public int TextureId { get; init; }
        public Vector2 Size { get; init; }
        public Vector2 Bearing { get; init; }
        public int Advance { get; init; }
    }
}
