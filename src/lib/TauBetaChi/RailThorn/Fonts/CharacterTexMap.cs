using OpenTK.Graphics.OpenGL4;

using ForgeWorks.RailThorn.Fonts.Native;

namespace ForgeWorks.RailThorn.Fonts;

internal unsafe class ChartacterTexMap : IDisposable
{
    private const int CHAR_COUNT = 128;
    private const uint PIXEL_SIZE_Y = 96;

    private readonly Face face;

    internal ChartacterTexMap(Face fontFace)
    {
        face = fontFace;
    }

    internal Font.Character[] MapFont()
    {
        //  disable byte-alingment restrictions
        GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

        Font.Character[] characters = new Font.Character[CHAR_COUNT];

        //  iterate first 128 ascii characters
        for (char c = (char)0; c < CHAR_COUNT; c++)
        {
            characters[c] = GenTexMap(c);
        }

        return characters;
    }

    private Font.Character GenTexMap(char c)
    {
        //  load glyph
        Glyph glyph = face.LoadGlyph(c, LoadFlags.Render, LoadTarget.Normal);
        GlyphBitmap bitmap = glyph.Bitmap;

        // create glyph texture
        int texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, texture);
        GL.TexImage2D(TextureTarget.Texture2D, 0,
            PixelInternalFormat.R8, (int)bitmap.Width, (int)bitmap.Rows, 0,
            PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);

        //  set texture parameters
        GL.TextureParameter(texture, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TextureParameter(texture, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TextureParameter(texture, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TextureParameter(texture, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        // build character
        return new()
        {
            Glyph = c,
            TextureId = texture,
            Size = (bitmap.Width, bitmap.Rows),
            Bearing = (glyph.BitmapLeft, glyph.BitmapTop),
            Advance = glyph.Advance.X
        };
    }

    public void Dispose()
    {
        //   ... ???
    }
}