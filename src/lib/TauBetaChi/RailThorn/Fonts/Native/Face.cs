
using System.Runtime.InteropServices;
using FreeTypeSharp;
using FreeTypeSharp.Native;

namespace ForgeWorks.RailThorn.Fonts.Native;

public unsafe class Face : INativeObject, IDisposable
{
    private const uint DEFAULT_PIXEL_HEIGHT = 32;

    //  internal reference to Face
    private readonly FT_FaceRec* face;

    private nint faceRef;
    private uint pixelHeight, pixelWidth;
    private bool disposed;

    internal FontLibrary FontLib { get; private init; }
    internal nint Reference => faceRef;
    nint INativeObject.Reference => Reference;

    public uint PixelHeight => pixelHeight;
    public uint PixelWidth => pixelWidth;
    public string FontFile { get; private init; }

    private Face(nint reference)
    {
        face = (FT_FaceRec*)(faceRef = reference);
    }

    /// <summary>
    /// Create a new <see cref="Face"/>.
    /// </summary>
    /// <param name="fontLib">The parent library.</param>
    /// <param name="fontFile">The path of the font file.</param>
    /// <param name="faceIndex">The index of the face to take from the file.</param>
    /// <param name="ftError">Out error status.</param>
    public static Face CreateFace(FontLibrary fontLib, string fontFile, int faceIndex, out FT_Error ftError)
    {
        ftError = FT.FT_New_Face(fontLib.Reference, fontFile, faceIndex, out nint reference);

        if (ftError != FT_Error.FT_Err_Ok)
        { throw new FreeTypeException(ftError); }

        Face face = new Face(reference)
        {
            FontLib = fontLib,
            FontFile = fontFile
        };

        face.SetPixelSize(0, DEFAULT_PIXEL_HEIGHT);

        return face;
    }

    /// <summary>
    /// This function calls <see cref="RequestSize"/> to request the nominal size (in pixels).
    /// </summary>
    /// <param name="width">The nominal width, in pixels.</param>
    /// <param name="height">The nominal height, in pixels</param>
    internal void SetPixelSize(uint width, uint height)
    {
        FT_Error ftError = FT.FT_Set_Pixel_Sizes(Reference, pixelWidth = width, pixelHeight = height);

        if (ftError != FT_Error.FT_Err_Ok)
        { throw new FreeTypeException(ftError); }
    }
    /// <summary>
    /// A function used to load a single glyph into the glyph slot of a face object, according to its character
    /// code.
    /// </summary>
    /// <remarks>
    /// This function simply calls <see cref="GetCharIndex"/> and <see cref="LoadGlyph"/>
    /// </remarks>
    /// <param name="charCode">
    /// The glyph's character code, according to the current charmap used in the face.
    /// </param>
    /// <param name="flags">
    /// A flag indicating what to load for this glyph. The <see cref="LoadFlags"/> constants can be used to control
    /// the glyph loading process (e.g., whether the outline should be scaled, whether to load bitmaps or not,
    /// whether to hint the outline, etc).
    /// </param>
    /// <param name="target">The target to OR with the flags.</param>
    internal unsafe Glyph LoadGlyph(uint charCode, LoadFlags flags, LoadTarget target)
    {
        FT_Error ftError = FT.FT_Load_Char(Reference, charCode, (int)flags | (int)target);

        if (ftError == FT_Error.FT_Err_Ok)
        {
            return new Glyph(face->glyph, this, FontLib);
        }

        throw new FreeTypeException(ftError);
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
                FT.FT_Done_Face(faceRef);
                Marshal.FreeHGlobal(faceRef);
            }

            faceRef = IntPtr.Zero;
        }
    }

    #endregion
}
