using OpenTK.Mathematics;

using FreeTypeSharp;

namespace ForgeWorks.RailThorn.Fonts;

public class Font
{
    private static readonly FreeTypeLibrary LOADER = FontManager.Instance.Loader;
    public static Font Default => FontManager.Instance.Default;

    private readonly Lazy<CharacterSet> _characterSet;

    private CharacterSet characterSet;

    private string FontFile { get; } = string.Empty;

    internal string Source => FontFile;

    public string Name { get; }
    public ICharacterSet CharacterSet => _characterSet.Value;
    public bool IsLoaded => characterSet?.Count > 0;

    public Font(string fontFile)
    {
        _characterSet = new(MapFont);

        FontFile = fontFile;
        Name = Path.GetFileNameWithoutExtension(fontFile);
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
    private CharacterSet MapFont()
    {
        if (!IsLoaded)
        {
            using (var mapper = new CharacterMapper(LOADER.Native, FontFile))
            {
                characterSet = new(Name, mapper.MapFont());
            }
        }

        return characterSet;
    }

    public struct Character
    {
        public char Glyph { get; set; }
        public uint TextureId { get; init; }
        public Vector2 Size { get; init; }
        public Vector2 Bearing { get; init; }
        public float Advance { get; init; }
    }
}
