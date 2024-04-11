#region MIT License
/*Copyright (c) 2012-2016 Robert Rouhani <robert.rouhani@gmail.com>

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

using System.Text;
using System.Runtime.InteropServices;

using ForgeWorks.RailThorn.Fonts.SharpFont.TrueType;
using ForgeWorks.RailThorn.Fonts.SharpFont.PostScript;


namespace ForgeWorks.RailThorn.Fonts.SharpFont;

/// <content>
/// This file contains all the raw FreeType2 function signatures.
/// </content>
public static partial class FT
{
	/// <summary>
	/// Defines the location of the FreeType DLL. Update SharpFont.dll.config if you change this!
	/// </summary>
	/// TODO: Use the same name for all platforms.
	private const string FreetypeDll = "freetype6";

	/// <summary>
	/// Defines the calling convention for P/Invoking the native freetype methods.
	/// </summary>
	private const CallingConvention CallConvention = CallingConvention.Cdecl;

	#region Core API

	#region FreeType Version

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Library_Version(IntPtr library, out int amajor, out int aminor, out int apatch);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static extern bool FT_Face_CheckTrueTypePatents(IntPtr face);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static extern bool FT_Face_SetUnpatentedHinting(IntPtr face, [MarshalAs(UnmanagedType.U1)] bool value);

	#endregion

	#region Base Interface

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Init_FreeType(out IntPtr alibrary);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Done_FreeType(IntPtr library);

	[DllImport(FreetypeDll, CallingConvention = CallConvention, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
	internal static extern Error FT_New_Face(IntPtr library, string filepathname, int face_index, out IntPtr aface);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_New_Memory_Face(IntPtr library, IntPtr file_base, int file_size, int face_index, out IntPtr aface);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Open_Face(IntPtr library, IntPtr args, int face_index, out IntPtr aface);

	[DllImport(FreetypeDll, CallingConvention = CallConvention, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
	internal static extern Error FT_Attach_File(IntPtr face, string filepathname);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Attach_Stream(IntPtr face, IntPtr parameters);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Reference_Face(IntPtr face);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Done_Face(IntPtr face);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Select_Size(IntPtr face, int strike_index);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Request_Size(IntPtr face, IntPtr req);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Set_Char_Size(IntPtr face, IntPtr char_width, IntPtr char_height, uint horz_resolution, uint vert_resolution);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Set_Pixel_Sizes(IntPtr face, uint pixel_width, uint pixel_height);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Load_Glyph(IntPtr face, uint glyph_index, int load_flags);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Load_Char(IntPtr face, uint char_code, int load_flags);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Set_Transform(IntPtr face, IntPtr matrix, IntPtr delta);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Render_Glyph(IntPtr slot, RenderMode render_mode);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_Kerning(IntPtr face, uint left_glyph, uint right_glyph, uint kern_mode, out FTVector26Dot6 akerning);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_Track_Kerning(IntPtr face, IntPtr point_size, int degree, out IntPtr akerning);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_Glyph_Name(IntPtr face, uint glyph_index, IntPtr buffer, uint buffer_max);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Get_Postscript_Name(IntPtr face);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Select_Charmap(IntPtr face, Encoding encoding);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Set_Charmap(IntPtr face, IntPtr charmap);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern int FT_Get_Charmap_Index(IntPtr charmap);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern uint FT_Get_Char_Index(IntPtr face, uint charcode);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern uint FT_Get_First_Char(IntPtr face, out uint agindex);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern uint FT_Get_Next_Char(IntPtr face, uint char_code, out uint agindex);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern uint FT_Get_Name_Index(IntPtr face, IntPtr glyph_name);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_SubGlyph_Info(IntPtr glyph, uint sub_index, out int p_index, out SubGlyphFlags p_flags, out int p_arg1, out int p_arg2, out FTMatrix p_transform);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern EmbeddingTypes FT_Get_FSType_Flags(IntPtr face);

	#endregion

	#region Glyph Variants

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern uint FT_Face_GetCharVariantIndex(IntPtr face, uint charcode, uint variantSelector);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern int FT_Face_GetCharVariantIsDefault(IntPtr face, uint charcode, uint variantSelector);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Face_GetVariantSelectors(IntPtr face);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Face_GetVariantsOfChar(IntPtr face, uint charcode);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Face_GetCharsOfVariant(IntPtr face, uint variantSelector);

	#endregion

	#region Glyph Management

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_Glyph(IntPtr slot, out IntPtr aglyph);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Glyph_Copy(IntPtr source, out IntPtr target);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Glyph_Transform(IntPtr glyph, ref FTMatrix matrix, ref FTVector delta);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Glyph_Get_CBox(IntPtr glyph, GlyphBBoxMode bbox_mode, out BBox acbox);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Glyph_To_Bitmap(ref IntPtr the_glyph, RenderMode render_mode, ref FTVector26Dot6 origin, [MarshalAs(UnmanagedType.U1)] bool destroy);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Done_Glyph(IntPtr glyph);

	#endregion

	#region Size Management

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_New_Size(IntPtr face, out IntPtr size);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Done_Size(IntPtr size);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Activate_Size(IntPtr size);

	#endregion

	#endregion

	#region Format-Specific API

	#region TrueType Tables

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Get_Sfnt_Table(IntPtr face, SfntTag tag);

	//TODO find FT_TRUETYPE_TAGS_H and create an enum for "tag"
	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Load_Sfnt_Table(IntPtr face, uint tag, int offset, IntPtr buffer, ref uint length);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static unsafe extern Error FT_Sfnt_Table_Info(IntPtr face, uint table_index, SfntTag* tag, out uint length);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern uint FT_Get_CMap_Language_ID(IntPtr charmap);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern int FT_Get_CMap_Format(IntPtr charmap);

	#endregion

	#region Type 1 Tables

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static extern bool FT_Has_PS_Glyph_Names(IntPtr face);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_PS_Font_Info(IntPtr face, out PostScript.Internal.FontInfoRec afont_info);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_PS_Font_Private(IntPtr face, out PostScript.Internal.PrivateRec afont_private);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern int FT_Get_PS_Font_Value(IntPtr face, DictionaryKeys key, uint idx, ref IntPtr value, int value_len);

	#endregion

	#region SFNT Names

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern uint FT_Get_Sfnt_Name_Count(IntPtr face);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_Sfnt_Name(IntPtr face, uint idx, out TrueType.Internal.SfntNameRec aname);

	#endregion

	#region BDF and PCF Files

	[DllImport(FreetypeDll, CallingConvention = CallConvention, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
	internal static extern Error FT_Get_BDF_Charset_ID(IntPtr face, out string acharset_encoding, out string acharset_registry);

	[DllImport(FreetypeDll, CallingConvention = CallConvention, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
	internal static extern Error FT_Get_BDF_Property(IntPtr face, string prop_name, out IntPtr aproperty);

	#endregion

	#region CID Fonts

	[DllImport(FreetypeDll, CallingConvention = CallConvention, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
	internal static extern Error FT_Get_CID_Registry_Ordering_Supplement(IntPtr face, out string registry, out string ordering, out int aproperty);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_CID_Is_Internally_CID_Keyed(IntPtr face, out byte is_cid);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_CID_From_Glyph_Index(IntPtr face, uint glyph_index, out uint cid);

	#endregion

	#region PFR Fonts

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_PFR_Metrics(IntPtr face, out uint aoutline_resolution, out uint ametrics_resolution, out IntPtr ametrics_x_scale, out IntPtr ametrics_y_scale);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_PFR_Kerning(IntPtr face, uint left, uint right, out FTVector avector);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_PFR_Advance(IntPtr face, uint gindex, out int aadvance);

	#endregion

	#region Window FNT Files

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_WinFNT_Header(IntPtr face, out IntPtr aheader);

	#endregion

	#region Font Formats

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Get_X11_Font_Format(IntPtr face);

	#endregion
	#endregion

	#region Support API

	#region Computations

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_MulDiv(IntPtr a, IntPtr b, IntPtr c);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_MulFix(IntPtr a, IntPtr b);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_DivFix(IntPtr a, IntPtr b);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_RoundFix(IntPtr a);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_CeilFix(IntPtr a);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_FloorFix(IntPtr a);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Vector_Transform(ref FTVector vec, ref FTMatrix matrix);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Matrix_Multiply(ref FTMatrix a, ref FTMatrix b);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Matrix_Invert(ref FTMatrix matrix);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Sin(IntPtr angle);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Cos(IntPtr angle);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Tan(IntPtr angle);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Atan2(IntPtr x, IntPtr y);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Angle_Diff(IntPtr angle1, IntPtr angle2);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Vector_Unit(out FTVector vec, IntPtr angle);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Vector_Rotate(ref FTVector vec, IntPtr angle);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Vector_Length(ref FTVector vec);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Vector_Polarize(ref FTVector vec, out IntPtr length, out IntPtr angle);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Vector_From_Polar(out FTVector vec, IntPtr length, IntPtr angle);

	#endregion

	#endregion

	#region Quick retrieval of advance values

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_Advance(IntPtr face, uint gIndex, LoadFlags load_flags, out IntPtr padvance);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Get_Advances(IntPtr face, uint start, uint count, LoadFlags load_flags, out IntPtr padvance);

	#endregion

	#region Bitmap Handling

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Bitmap_New(IntPtr abitmap);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Bitmap_Copy(IntPtr library, IntPtr source, IntPtr target);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Bitmap_Embolden(IntPtr library, IntPtr bitmap, IntPtr xStrength, IntPtr yStrength);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Bitmap_Convert(IntPtr library, IntPtr source, IntPtr target, int alignment);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_GlyphSlot_Own_Bitmap(IntPtr slot);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Bitmap_Done(IntPtr library, IntPtr bitmap);

	#endregion

	#region Module Management

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Add_Module(IntPtr library, IntPtr clazz);

	[DllImport(FreetypeDll, CallingConvention = CallConvention, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
	internal static extern IntPtr FT_Get_Module(IntPtr library, string module_name);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Remove_Module(IntPtr library, IntPtr module);

	[DllImport(FreetypeDll, CallingConvention = CallConvention, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
	internal static extern Error FT_Property_Set(IntPtr library, string module_name, string property_name, IntPtr value);

	[DllImport(FreetypeDll, CallingConvention = CallConvention, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
	internal static extern Error FT_Property_Get(IntPtr library, string module_name, string property_name, IntPtr value);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Reference_Library(IntPtr library);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_New_Library(nint memory, out IntPtr alibrary);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Done_Library(IntPtr library);

	//TODO figure out the method signature for debug_hook. (FT_DebugHook_Func)
	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Set_Debug_Hook(IntPtr library, uint hook_index, IntPtr debug_hook);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern void FT_Add_Default_Modules(IntPtr library);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern IntPtr FT_Get_Renderer(IntPtr library, GlyphFormat format);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Set_Renderer(IntPtr library, IntPtr renderer, uint num_params, IntPtr parameters);

	#endregion

	#region GZIP Streams

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Stream_OpenGzip(IntPtr stream, IntPtr source);

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Gzip_Uncompress(IntPtr memory, IntPtr output, ref IntPtr output_len, IntPtr input, IntPtr input_len);

	#endregion

	#region LZW Streams

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Stream_OpenLZW(IntPtr stream, IntPtr source);

	#endregion

	#region BZIP2 Streams

	[DllImport(FreetypeDll, CallingConvention = CallConvention)]
	internal static extern Error FT_Stream_OpenBzip2(IntPtr stream, IntPtr source);

	#endregion
}
