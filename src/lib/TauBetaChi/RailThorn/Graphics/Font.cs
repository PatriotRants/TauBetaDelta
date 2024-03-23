using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using FreeTypeSharp;
using FreeTypeSharp.Native;

using ForgeWorks.RailThorn.Graphics;

namespace ForgeWorks.RailThorn.Fonts;

internal delegate nint LoadFont(nint loader, Font font);

public class Font
{
    private static readonly FreeTypeLibrary LOADER = FontManager.Instance.Loader;

    public static Font Default => FontManager.Instance.Default;

    private Dictionary<uint, Character> Characters { get; } = new();
    private string FontFile { get; } = string.Empty;
    private FreeTypeFaceFacade FontFace { get; set; }

    internal LoadFont LoadFont
    { get; init; }
    internal string Source => FontFile;

    public string Name { get; }
    public bool IsLoaded => FontFace?.Face != IntPtr.Zero;

    public Font(string fontFile)
    {
        FontFile = fontFile;
        Name = Path.GetFileNameWithoutExtension(fontFile);
    }

    public static implicit operator Font(string fontName)
    {
        return FontManager.Instance.GetFont(fontName);
    }

    /// <summary>
    /// Prepares the font for use
    /// </summary>
    public void Use()
    {
        if (!IsLoaded)
        {
            FontFace = new FreeTypeFaceFacade(LOADER, LoadFont(LOADER.Native, this));
            MapFont();
        }
    }

    private void MapFont()
    {
        //  disable byte-alingment restrictions
        GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

        /* **
            following the tut at https://learnopengl.com/In-Practice/Text-Rendering
            ... so I don't know if I'm duplicating something FreeTypeSharp has already implemented or not
         ** */
        //  iterate first 128 ascii characters
        for (uint c = 0; c < 128; c++)
        {
            FT_Error ftErr = FT.FT_Load_Char(FontFace.Face, c, FT.FT_LOAD_RENDER);

            if (ftErr != FT_Error.FT_Err_Ok)
            {
                throw new InvalidOperationException($"Failed to load glyph {(char)c}");
            }

            Characters.Add(c, BuildCharacterBitmap(c));

        }
    }

    private Character BuildCharacterBitmap(uint c)
    {
        // generate texture
        GL.GenTextures(1, out uint texture);
        GL.BindTexture(TextureTarget.Texture2D, texture);
        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.CompressedRed,
            (int)FontFace.GlyphBitmap.width,
            (int)FontFace.GlyphBitmap.rows,
            0,
            PixelFormat.Red,
            PixelType.UnsignedByte,
            FontFace.GlyphBitmap.buffer
        );
        // set texture options
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        // build character for later use
        return new()
        {
            TextureId = texture,
            Size = (FontFace.GlyphMetricWidth, (int)FontFace.GlyphBitmap.rows), // glm::ivec2(face->glyph->bitmap.width, face->glyph->bitmap.rows),
            Bearing = (FontFace.GlyphBitmapLeft, FontFace.GlyphBitmapTop),      // glm::ivec2(face->glyph->bitmap_left, face->glyph->bitmap_top),
            Advance = FontFace.GlyphMetricHorizontalAdvance // face->glyph->advance.x
        };
    }

    private struct Character
    {
        public uint TextureId { get; init; }
        public Vector2i Size { get; init; }
        public Vector2i Bearing { get; init; }
        public int Advance { get; init; }
    }
}
