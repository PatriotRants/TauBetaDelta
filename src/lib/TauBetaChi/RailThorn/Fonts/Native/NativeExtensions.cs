using System.Runtime.InteropServices;

using FreeTypeSharp.Native;

namespace ForgeWorks.RailThorn.Fonts.Native;

internal static class NativeExtensions
{
    public static string GetErrorMessage(this FT_Error ftError)
    {
        switch (ftError)
        {
            case FT_Error.FT_Err_Ok: return "No error.";
            case FT_Error.FT_Err_Cannot_Open_Resource: return "Cannot open resource.";
            case FT_Error.FT_Err_Unknown_File_Format: return "Unknown file format.";
            case FT_Error.FT_Err_Invalid_File_Format: return "Broken file.";
            case FT_Error.FT_Err_Invalid_Version: return "Invalid FreeType version.";
            case FT_Error.FT_Err_Lower_Module_Version: return "Module version is too low.";
            case FT_Error.FT_Err_Invalid_Argument: return "Invalid argument.";
            case FT_Error.FT_Err_Unimplemented_Feature: return "Unimplemented feature.";
            case FT_Error.FT_Err_Invalid_Table: return "Broken table.";
            case FT_Error.FT_Err_Invalid_Offset: return "Broken offset within table.";
            case FT_Error.FT_Err_Array_Too_Large: return "Array allocation size too large.";
            case FT_Error.FT_Err_Invalid_Glyph_Index: return "Invalid glyph index.";
            case FT_Error.FT_Err_Invalid_Character_Code: return "Invalid character code.";
            case FT_Error.FT_Err_Invalid_Glyph_Format: return "Unsupported glyph image format.";
            case FT_Error.FT_Err_Cannot_Render_Glyph: return "Cannot render this glyph format.";
            case FT_Error.FT_Err_Invalid_Outline: return "Invalid outline.";
            case FT_Error.FT_Err_Invalid_Composite: return "Invalid composite glyph.";
            case FT_Error.FT_Err_Too_Many_Hints: return "Too many hints.";
            case FT_Error.FT_Err_Invalid_Pixel_Size: return "Invalid pixel size.";
            case FT_Error.FT_Err_Invalid_Handle: return "Invalid object handle.";
            case FT_Error.FT_Err_Invalid_Library_Handle: return "Invalid library handle.";
            case FT_Error.FT_Err_Invalid_Driver_Handle: return "Invalid module handle.";
            case FT_Error.FT_Err_Invalid_Face_Handle: return "Invalid face handle.";
            case FT_Error.FT_Err_Invalid_Size_Handle: return "Invalid size handle.";
            case FT_Error.FT_Err_Invalid_Slot_Handle: return "Invalid glyph slot handle.";
            case FT_Error.FT_Err_Invalid_CharMap_Handle: return "Invalid charmap handle.";
            case FT_Error.FT_Err_Invalid_Cache_Handle: return "Invalid cache manager handle.";
            case FT_Error.FT_Err_Invalid_Stream_Handle: return "Invalid stream handle.";
            case FT_Error.FT_Err_Too_Many_Drivers: return "Too many modules.";
            case FT_Error.FT_Err_Too_Many_Extensions: return "Too many extensions.";
            case FT_Error.FT_Err_Out_Of_Memory: return "Out of memory.";
            case FT_Error.FT_Err_Unlisted_Object: return "Unlisted object.";
            case FT_Error.FT_Err_Cannot_Open_Stream: return "Cannot open stream.";
            case FT_Error.FT_Err_Invalid_Stream_Seek: return "Invalid stream seek.";
            case FT_Error.FT_Err_Invalid_Stream_Skip: return "Invalid stream skip.";
            case FT_Error.FT_Err_Invalid_Stream_Read: return "Invalid stream read.";
            case FT_Error.FT_Err_Invalid_Stream_Operation: return "Invalid stream operation.";
            case FT_Error.FT_Err_Invalid_Frame_Operation: return "Invalid frame operation.";
            case FT_Error.FT_Err_Nested_Frame_Access: return "Nested frame access.";
            case FT_Error.FT_Err_Invalid_Frame_Read: return "Invalid frame read.";
            case FT_Error.FT_Err_Raster_Uninitialized: return "Raster uninitialized.";
            case FT_Error.FT_Err_Raster_Corrupted: return "Raster corrupted.";
            case FT_Error.FT_Err_Raster_Overflow: return "Raster overflow.";
            case FT_Error.FT_Err_Raster_Negative_Height: return "Negative height while rastering.";
            case FT_Error.FT_Err_Too_Many_Caches: return "Too many registered caches.";
            case FT_Error.FT_Err_Invalid_Opcode: return "Invalid opcode.";
            case FT_Error.FT_Err_Too_Few_Arguments: return "Too few arguments.";
            case FT_Error.FT_Err_Stack_Overflow: return "Stack overflow.";
            case FT_Error.FT_Err_Code_Overflow: return "Code overflow.";
            case FT_Error.FT_Err_Bad_Argument: return "Bad argument.";
            case FT_Error.FT_Err_Divide_By_Zero: return "Division by zero.";
            case FT_Error.FT_Err_Invalid_Reference: return "Invalid reference.";
            case FT_Error.FT_Err_Debug_OpCode: return "Found debug opcode.";
            case FT_Error.FT_Err_ENDF_In_Exec_Stream: return "Found ENDF opcode in execution stream.";
            case FT_Error.FT_Err_Nested_DEFS: return "Nested DEFS.";
            case FT_Error.FT_Err_Invalid_CodeRange: return "Invalid code range.";
            case FT_Error.FT_Err_Execution_Too_Long: return "Execution context too long.";
            case FT_Error.FT_Err_Too_Many_Function_Defs: return "Too many function definitions.";
            case FT_Error.FT_Err_Too_Many_Instruction_Defs: return "Too many instruction definitions.";
            case FT_Error.FT_Err_Table_Missing: return "SFNT font table missing.";
            case FT_Error.FT_Err_Horiz_Header_Missing: return "Horizontal header (hhea) table missing.";
            case FT_Error.FT_Err_Locations_Missing: return "Locations (loca) table missing.";
            case FT_Error.FT_Err_Name_Table_Missing: return "Name table missing.";
            case FT_Error.FT_Err_CMap_Table_Missing: return "Character map (cmap) table missing.";
            case FT_Error.FT_Err_Hmtx_Table_Missing: return "Horizontal metrics (hmtx) table missing.";
            case FT_Error.FT_Err_Post_Table_Missing: return "PostScript (post) table missing.";
            case FT_Error.FT_Err_Invalid_Horiz_Metrics: return "Invalid horizontal metrics.";
            case FT_Error.FT_Err_Invalid_CharMap_Format: return "Invalid character map (cmap) format.";
            case FT_Error.FT_Err_Invalid_PPem: return "Invalid ppem value.";
            case FT_Error.FT_Err_Invalid_Vert_Metrics: return "Invalid vertical metrics.";
            case FT_Error.FT_Err_Could_Not_Find_Context: return "Could not find context.";
            case FT_Error.FT_Err_Invalid_Post_Table_Format: return "Invalid PostScript (post) table format.";
            case FT_Error.FT_Err_Invalid_Post_Table: return "Invalid PostScript (post) table.";
            case FT_Error.FT_Err_Syntax_Error: return "Opcode syntax error.";
            case FT_Error.FT_Err_Stack_Underflow: return "Argument stack underflow.";
            case FT_Error.FT_Err_Ignore: return "Ignore this error.";
            case FT_Error.FT_Err_No_Unicode_Glyph_Name: return "No Unicode glyph name found.";
            case FT_Error.FT_Err_Missing_Startfont_Field: return "`STARTFONT' field missing.";
            case FT_Error.FT_Err_Missing_Font_Field: return "`FONT' field missing.";
            case FT_Error.FT_Err_Missing_Size_Field: return "`SIZE' field missing.";
            case FT_Error.FT_Err_Missing_Fontboundingbox_Field: return "`FONTBOUNDINGBOX' field missing.";
            case FT_Error.FT_Err_Missing_Chars_Field: return "`CHARS' field missing.";
            case FT_Error.FT_Err_Missing_Startchar_Field: return "`STARTCHAR' field missing.";
            case FT_Error.FT_Err_Missing_Encoding_Field: return "`ENCODING' field missing.";
            case FT_Error.FT_Err_Missing_Bbx_Field: return "`BBX' field missing.";
            case FT_Error.FT_Err_Bbx_Too_Big: return "`BBX' too big.";
            case FT_Error.FT_Err_Corrupted_Font_Header: return "Font header corrupted or missing fields.";
            case FT_Error.FT_Err_Corrupted_Font_Glyphs: return "Font glyphs corrupted or missing fields.";
            default: return "Encountered an unknown error. Most likely this is a new error that hasn't been included in SharpFont yet. Error:" + (int)ftError;
        }
    }

    /// <summary>
    /// A generic wrapper for <see cref="Marshal.PtrToStructure(IntPtr, Type)"/>.
    /// </summary>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <param name="reference">The pointer that holds the struct.</param>
    /// <returns>A marshalled struct.</returns>
    internal static T ToStructure<T>(this IntPtr reference)
    {
        return (T)Marshal.PtrToStructure(reference, typeof(T));
    }
    /// A common pattern in SharpFont is to pass a pointer to a memory address inside of a struct. This method
    /// works for all cases and provides a generic API.
    /// </summary>
    /// <see cref="Marshal.OffsetOf"/>
    /// <typeparam name="T">The type of the struct to take an offset from.</typeparam>
    /// <param name="start">A pointer to the start of a struct.</param>
    /// <param name="fieldName">The name of the field to get an offset to.</param>
    /// <returns><code>start</code> + the offset of the <code>fieldName</code> field in <code>T</code>.</returns>
    internal static IntPtr AbsoluteOffsetOf<T>(this IntPtr start, string fieldName)
    {
        return new IntPtr(start.ToInt64() + Marshal.OffsetOf(typeof(T), fieldName).ToInt64());
    }
}