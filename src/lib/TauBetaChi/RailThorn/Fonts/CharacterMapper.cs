using OpenTK.Graphics.OpenGL4;
using FreeTypeSharp.Native;
using static FreeTypeSharp.Native.FT;

namespace ForgeWorks.RailThorn.Fonts;

internal unsafe class CharacterMapper : IDisposable
{
    private const int CHAR_COUNT = 128;
    private const uint PIXEL_SIZE_Y = 96;

    private readonly nint face;
    private readonly FT_FaceRec* faceRec;

    internal FT_Error Error { get; private set; }

    internal CharacterMapper(nint ftLib, string trueTypeName)
    {
        if ((Error = FT_New_Face(ftLib, trueTypeName, 0, out face)) == FT_Error.FT_Err_Ok)
        {
            faceRec = (FT_FaceRec*)face;
            FT_Set_Pixel_Sizes(face, 0, PIXEL_SIZE_Y);
        }
        else
        {
            //  handle exception (FontLoadException ...???)
        }
    }

    internal Font.Character[] MapFont()
    {
        //  disable byte-alingment restrictions
        GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

        Font.Character[] characters = new Font.Character[CHAR_COUNT];

        //  iterate first 128 ascii characters
        for (char c = (char)0; c < CHAR_COUNT; c++)
        {
            if ((Error = FT.FT_Load_Char(face, c, FT_LOAD_RENDER)) != FT_Error.FT_Err_Ok)
            {
                //  handle exception (FontMappingException ...???)

                break;
            }

            characters[c] = BuildCharacterBitmap(c);
        }

        if (Error != FT_Error.FT_Err_Ok)
        {
            //  we return an empty array if there was an error mapping characters
            characters = new Font.Character[] { };
        }

        return characters;
    }

    private Font.Character BuildCharacterBitmap(char c)
    {
        const float SCALE_FACTOR = 1f / PIXEL_SIZE_Y;

        // generate texture
        GL.GenTextures(1, out uint texture);
        GL.BindTexture(TextureTarget.Texture2D, texture);

        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.CompressedRed,
            (int)faceRec->glyph->bitmap.width,
            (int)faceRec->glyph->bitmap.rows,
            0,
            PixelFormat.Red,
            PixelType.UnsignedByte,
            faceRec->glyph->bitmap.buffer
        );

        // set texture options
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        //  calculate parameters
        float width = SCALE_FACTOR * faceRec->glyph->bitmap.width;
        float height = SCALE_FACTOR * faceRec->glyph->bitmap.rows;
        float bearingX = SCALE_FACTOR * faceRec->glyph->bitmap_left;
        float bearingY = SCALE_FACTOR * faceRec->glyph->bitmap_top;
        float advance = SCALE_FACTOR * faceRec->glyph->advance.x / 30;

        // build character
        return new()
        {
            Glyph = c,
            TextureId = texture,
            Size = (width, height),
            Bearing = (bearingX, bearingY),
            Advance = advance
        };
    }

    public void Dispose()
    {
        FT_Done_Face(face);
    }
}