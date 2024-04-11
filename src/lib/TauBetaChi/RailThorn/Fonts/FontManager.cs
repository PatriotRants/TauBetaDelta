using ForgeWorks.ShowBird.Messaging;

using ForgeWorks.TauBetaDelta.Logging;
using ForgeWorks.TauBetaDelta.Extensibility;
using ForgeWorks.RailThorn.Fonts.Native;

namespace ForgeWorks.RailThorn.Fonts;

public class FontManager : IUnloadable
{
    /* **
        We have one of 2 ways to go:
        1. determine what platform we are using and locate the correct platform, OR
        2. create a ./fonts directory local to this installation

        For the time being, I am going to hardcode the truetype fonts directory
    ** */
    private const string TTF_LIN_SYS_FONTS_DIR = "/usr/share/fonts/truetype";
    private const string TTF_SYS_LIN_DEFAULT = "LiberationMono-Regular";
    private const string TTF_FONTS_DIR = "./fonts";

    private static readonly Lazy<FontManager> INSTANCE = new(() => new());

    private readonly Dictionary<string, Font> fonts = new();

    private ResourceLogger Log { get; } = LoggerManager.Instance.Post;
    private (LoadStatus condition, ErrorCode errCode, string message) Status { get; set; }
    private FontLibrary FontLib { get; set; }

    internal static FontManager Instance => INSTANCE.Value;

    public Font Default => GetDefaultFont();
    public string FontDirectory { get; } = TTF_FONTS_DIR;
    public string Info => $"[{Status.condition}.{Status.errCode}] {Status.message}";

    /// <summary>
    /// When FontManager is instantiated, it will automagically load the default font.
    /// </summary>
    private FontManager()
    {
        Status = (LoadStatus.Okay, ErrorCode.Ok, "FontManager Okay");

        //  verify system fonts .ttf directory
        if (!Directory.Exists(TTF_LIN_SYS_FONTS_DIR))
        {
            Status = (LoadStatus.Error, ErrorCode.DirNotFound, $"[TTF Sys Dir] Error: [{ErrorCode.DirNotFound}] {TTF_LIN_SYS_FONTS_DIR}");
        }
        else
        {
            FontLib = new(TTF_FONTS_DIR);

            //  get the default system font
            var defaultFont = FontFiles(TTF_LIN_SYS_FONTS_DIR)
                .Where(f => Path.GetFileNameWithoutExtension(f) == TTF_SYS_LIN_DEFAULT)
                .FirstOrDefault();

            if (TryGetFont(FontLib, defaultFont, out Font font))
            {
                fonts.Add(font.Name, font);
            }
        }

        if (!Directory.Exists(FontDirectory))
        {
            Status = (LoadStatus.Error, ErrorCode.DirNotFound, $"[{nameof(FontManager)}] Font directory '{FontDirectory}' does not exist");
        }
        else
        {
            Status = (LoadStatus.Okay, ErrorCode.Ok, $"[{nameof(FontManager)}] Ready Font directory '{FontDirectory}'");
        }


        Log(Status.condition, Info);
    }

    /// <summary>
    /// Loads all custom fonts and maps the default font. Loaded fonts will lazily map character sets
    /// </summary>
    /// <param name="updateAgent"></param>
    /// <returns></returns>
    public int LoadFonts(UpdateAgent updateAgent)
    {
        int count = 0;
        //  default font is the only font that loads immediately
        Font defaultFont = GetDefaultFont();
        //  causes lazy loading of the default font character set
        _ = defaultFont.CharacterSet.Count > 0;
        updateAgent($"[FontManager] Default Font: {defaultFont.Name} {(defaultFont.IsLoaded ? "LOADED" : "ERROR")}");

        foreach (var font in Fonts(FontLib, FontDirectory))
        {
            updateAgent($"Font: {font.Name}");
            fonts.Add(font.Name, font);

            ++count;
        }

        return count;
    }
    public Font GetFont(string fontName)
    {
        Font font = null;
        if (!fonts.TryGetValue(fontName, out font))
        {
            Log(LoadStatus.Error, $"Font [{fontName}] not found");
        }

        return font;
    }
    private static IEnumerable<Font> Fonts(FontLibrary fontLib, string directory)
    {
        //  assumes each .ttf is in its own folder
        foreach (var fontDir in Directory.GetDirectories(directory))
        {
            string fontFile = Directory.GetFiles(fontDir)
                .Where(f => Path.GetExtension(f) == ".ttf")
                .FirstOrDefault();

            if (string.IsNullOrEmpty(fontFile))
            { continue; }

            if (TryGetFont(fontLib, fontFile, out Font font))
            { yield return font; }
        }
    }
    private static IEnumerable<string> FontFiles(string directory)
    {
        foreach (var fontDir in Directory.GetDirectories(directory))
        {
            string fontFile = Directory.GetFiles(fontDir)
                .Where(f => Path.GetExtension(f) == ".ttf")
                .FirstOrDefault();

            if (string.IsNullOrEmpty(fontFile))
            {
                continue;
            }

            yield return fontFile;
        }
    }
    private static bool TryGetFont(FontLibrary fontLib, string fontFile, out Font font)
    {
        ResourceStatus resStatus = ResourceStatus.Okay;
        font = null;

        var face = fontLib.LoadFace(fontFile, out resStatus);

        if (resStatus != ResourceStatus.Okay)
        {
            string logEntry = $"[FontLibrary] {(resStatus == ResourceStatus.Error ? "Error" : string.Empty)}: {resStatus} ({fontFile})";
            LoggerManager.Instance.Post(LoadStatus.Error, logEntry);
        }

        font = new TrueTypeFont(face);

        return font != null;
    }

    private Font GetDefaultFont()
    {
        if (!fonts.TryGetValue(TTF_SYS_LIN_DEFAULT, out Font font))
        {
            //  then return the first font in the collection
            font = fonts.Values.First();
        }

        return font;
    }
    public void Unload(AutoResetEvent taskEvent)
    {
        fonts.Clear();
        FontLib.Dispose();
    }
}