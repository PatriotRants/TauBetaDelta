using FreeTypeSharp;
using FreeTypeSharp.Native;
using static FreeTypeSharp.Native.FT;

using ForgeWorks.GlowFork;

using ForgeWorks.ShowBird.Messaging;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.RailThorn.Fonts;

public class FontManager : IDisposable
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

    private readonly FreeTypeLibrary loader;

    private ResourceLogger Log { get; } = LoggerManager.Instance.Post;
    private (LoadStatus condition, ErrorCode errCode, string message) Status { get; set; }
    private readonly Dictionary<string, Font> fonts = new();

    internal static FontManager Instance => INSTANCE.Value;

    internal FreeTypeLibrary Loader => loader;

    public Font Default => GetDefaultFont();
    public string FontDirectory { get; } = TTF_FONTS_DIR;
    public string Info => $"[{Status.condition}.{Status.errCode}] {Status.message}";

    private FontManager()
    {
        Status = (LoadStatus.Okay, ErrorCode.NoErr, "FontManager Okay");

        //  initialize FreeType
        loader = new FreeTypeLibrary();

        //  verify system fonts .ttf directory
        if (!Directory.Exists(TTF_LIN_SYS_FONTS_DIR))
        {
            Status = (LoadStatus.Error, ErrorCode.DirNotFound, $"[TTF Sys Dir] Error: [{ErrorCode.DirNotFound}] {TTF_LIN_SYS_FONTS_DIR}");
        }
        else
        {
            //  get the default system font
            var defaultFont = FontFiles(TTF_LIN_SYS_FONTS_DIR)
                .Where(f => Path.GetFileNameWithoutExtension(f) == TTF_SYS_LIN_DEFAULT)
                .FirstOrDefault();
            if (TryGetFont(loader.Native, defaultFont, out Font font))
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
            Status = (LoadStatus.Okay, ErrorCode.NoErr, $"[{nameof(FontManager)}] Ready Font directory '{FontDirectory}'");
        }


        Log(Status.condition, Info);
    }

    public int LoadFonts(UpdateAgent updateAgent)
    {
        int count = 0;
        //  default font is the only font that loads immediately
        Font defaultFont = GetDefaultFont();
        defaultFont.Use();
        updateAgent($"[FontManager] Default Font: {defaultFont.Name} {(defaultFont.IsLoaded ? "LOADED" : "ERROR")}");

        foreach (var font in Fonts(FontDirectory))
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
    private static IEnumerable<Font> Fonts(string directory)
    {
        //  assumes each .ttf is in its own folder
        foreach (var fontDir in Directory.GetDirectories(directory))
        {
            string fontFile = Directory.GetFiles(fontDir)
                .Where(f => Path.GetExtension(f) == ".ttf")
                .FirstOrDefault();

            if (string.IsNullOrEmpty(fontFile))
            {
                continue;
            }

            yield return new Font(fontFile);
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
    private static nint LoadFont(nint loader, Font font)
    {
        FT_Error ftErr = FT_Error.FT_Err_Ignore;

        if ((ftErr = FT_New_Face(loader, font.Source, 0, out nint face)) != FT_Error.FT_Err_Ok)
        {
            LoggerManager.Instance.Post(LoadStatus.Error, $"[FreeType] Error: {ftErr} ({font.Source})");
        }

        return face;
    }
    private static bool TryGetFont(nint loadere, string fontFile, out Font font)
    {
        FT_Error ftErr = FT_Error.FT_Err_Ignore;
        font = null;

        if ((ftErr = FT_New_Face(loadere, fontFile, 0, out nint face)) != FT_Error.FT_Err_Ok)
        {
            LoggerManager.Instance.Post(LoadStatus.Error, $"[FreeType] Error: {ftErr} ({fontFile})");
        }
        else
        {
            font = new Font(fontFile)
            {
                LoadFont = LoadFont
            };
        }

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
    public void Dispose()
    {

    }
}