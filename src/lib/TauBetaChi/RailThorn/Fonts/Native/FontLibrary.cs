#region MIT License
/*Copyright (c) 2012-2014, 2016 Robert Rouhani <robert.rouhani@gmail.com>

SharpFont based on Tao.FreeType, Copyright (c) 2003-2007 Tao Framework Team

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
#endregion

using FreeTypeSharp;
using FreeTypeSharp.Native;

using ForgeWorks.TauBetaDelta.Logging;
using static ForgeWorks.RailThorn.Fonts.Native.Face;

namespace ForgeWorks.RailThorn.Fonts.Native;

/// <summary><para>
/// A handle to a FreeType library instance. Each ‘library’ is completely independent from the others; it is the
/// ‘root’ of a set of objects like fonts, faces, sizes, etc.
/// </para><para>
/// It also embeds a memory manager (see <see cref="Memory"/>), as well as a scan-line converter object (see
/// <see cref="Raster"/>).
/// </para><para>
/// For multi-threading applications each thread should have its own <see cref="FontLibrary"/> object.
/// </para></summary>
public sealed class FontLibrary : IDisposable
{
	#region Fields

	private nint libRef = nint.Zero;
	private bool isDisposed;

	#endregion

	#region Properties
	internal nint Reference
	{
		get
		{
			if (!isDisposed)
			{ return libRef; }

			throw new ObjectDisposedException("Reference", "Cannot access a disposed object.");
		}
	}
	internal string Directory { get; } = string.Empty;

	/// <summary>
	/// Gets a value indicating whether the object has been disposed.
	/// </summary>
	public bool IsDisposed => isDisposed;
	/// <summary>
	/// Gets the version of the FreeType library being used.
	/// </summary>
	public Version Version => GetFTVersion(Reference, isDisposed);
	#endregion

	#region Constructors
	/// <summary>
	/// Initializes a new instance of the <see cref="FontLibrary"/> class.
	/// </summary>
	/// <param name="memory">A custom FreeType memory manager.</param>
	/// <remarks>
	/// SharpFont assumes that you have the correct version of FreeType for your operating system and processor
	/// architecture. If you get a <see cref="BadImageFormatException"/> on Windows, you may be trying to
	/// execute the program as a 64-bit process with a 32-bit installation of FreeType (or vice versa).
	/// See the SharpFont.Examples project for how to handle this situation.
	/// </remarks>
	public FontLibrary(string fontDirectory)
	{
		FT_Error err = FT.FT_Init_FreeType(out libRef);

		if (err == FT_Error.FT_Err_Ok)
		{
			Directory = fontDirectory;
			return;
		}

		throw new FreeTypeException(err);
	}

	/// <summary>
	/// Finalizes an instance of the <see cref="FontLibrary"/> class.
	/// </summary>
	~FontLibrary()
	{
		Dispose(false);
	}

	#endregion

	#region Methods

	#region Base Interface
	/// <summary>
	/// This function calls <see cref="OpenFace"/> to open a font by its pathname.
	/// </summary>
	/// <param name="fontFile">A path to the font file.</param>
	/// <param name="resourceStatus">Resource load status.</param>
	/// <param name="faceIndex">The index of the face within the font. The first face has index 0.</param>
	/// <returns>
	/// A handle to a new face object. If ‘faceIndex’ is greater than or equal to zero, it must be non-NULL.
	/// </returns>
	/// <see cref="OpenFace"/>
	public Face LoadFace(string fontFile, out ResourceStatus resourceStatus, int faceIndex = 0)
	{
		resourceStatus = ResourceStatus.Okay;
		FT_Error ftError = FT_Error.FT_Err_Ok;
		LogLevel logLevel = LogLevel.Default;

		if (!isDisposed)
		{
			Face face = CreateFace(this, fontFile, faceIndex, out ftError);

			switch (ftError)
			{
				case FT_Error.FT_Err_Ok:
					resourceStatus = ResourceStatus.Okay;

					break;
				case FT_Error.FT_Err_Ignore:
					resourceStatus = ResourceStatus.Ignore;

					break;
				default:
					resourceStatus = ResourceStatus.Error;
					logLevel = LogLevel.Error;

					break;
			}

			LoggerManager.Instance.Post(logLevel, $"[{nameof(LoadFace)}] ({fontFile}){ftError.GetErrorMessage()}");

			return face;
		}

		throw new ObjectDisposedException("FontLibrary", "Cannot access a disposed object.");
	}
	#endregion

	/// <summary>
	/// Disposes the FontLibrary.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!isDisposed)
		{
			disposing = isDisposed = true;

			FT_Error err = FT.FT_Done_FreeType(libRef);
			libRef = IntPtr.Zero;
		}
	}
	private static Version GetFTVersion(nint libRef, bool isDisposed)
	{
		if (!isDisposed)
		{
			int major, minor, patch;
			FT.FT_Library_Version(libRef, out major, out minor, out patch);

			return new Version(major, minor, patch);
		}

		throw new ObjectDisposedException("Version", "Cannot access a disposed object.");
	}
	#endregion


}
