using System.Runtime.InteropServices;
using FreeTypeSharp.Native;
using OpenTK.Mathematics;

namespace ForgeWorks.RailThorn.Fonts.Native;

public unsafe sealed class Glyph : INativeObject, IDisposable
{
    private readonly FT_GlyphSlotRec* glyph;

    private nint glyphRef;
    private bool disposed;

    nint INativeObject.Reference => glyphRef;

    public Face Face { get; }
    public FontLibrary FontLib { get; }
    public GlyphBitmap Bitmap => GetGlyphBitmap();
    /// <summary>
    /// Gets the advance. This shorthand is, depending on <see cref="LoadFlags.IgnoreTransform"/>, the transformed
    /// advance width for the glyph (in 26.6 fractional pixel format). As specified with
    /// <see cref="LoadFlags.VerticalLayout"/>, it uses either the ‘horiAdvance’ or the ‘vertAdvance’ value of
    /// ‘metrics’ field.
    /// </summary>
    public Vector2i Advance
    {
        get
        {
            return ((int)glyph->advance.x, (int)glyph->advance.y);
        }
    }
    /// <summary>
    /// Gets the bitmap's left bearing expressed in integer pixels. Of course, this is only valid if the format is
    /// <see cref="GlyphFormat.Bitmap"/>.
    /// </summary>
    public int BitmapLeft
    {
        get
        {
            return glyph->bitmap_left;
        }
    }
    /// <summary>
    /// Gets the bitmap's top bearing expressed in integer pixels. Remember that this is the distance from the
    /// baseline to the top-most glyph scanline, upwards y coordinates being positive.
    /// </summary>
    public int BitmapTop
    {
        get
        {
            return glyph->bitmap_top;
        }
    }

    public Glyph(FT_GlyphSlotRec* reference, Face face, FontLibrary fontLib)
    {
        glyphRef = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FT_GlyphSlotRec)));
        glyph = reference;
        Face = face;
        FontLib = fontLib;
    }

    private GlyphBitmap GetGlyphBitmap()
    {
        return new GlyphBitmap(glyphRef.AbsoluteOffsetOf<FT_GlyphSlotRec>("bitmap"), glyph->bitmap, FontLib);
    }

    #region IDisposable

    /// <summary>
    /// Disposes an instance of the <see cref="GlyphBitmap"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!disposed)
        {
            disposed = true;

            if (true)
            {
                FT.FT_Done_Glyph(glyphRef);
                Marshal.FreeHGlobal(glyphRef);
            }

            glyphRef = IntPtr.Zero;
        }
    }

    #endregion

}