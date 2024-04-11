
namespace ForgeWorks.RailThorn.Fonts.SharpFont;

/// <summary>
/// Provide a consistent means for using pointers as references.
/// </summary>
public abstract class NativeObject : INativeObject
{
	private readonly nint objectRef;

	IntPtr INativeObject.Reference => objectRef;

	/// <summary>
	/// Construct a new NativeObject and assign the reference.
	/// </summary>
	/// <param name="refObject"></param>
	protected NativeObject(IntPtr reference)
	{
		objectRef = reference;
	}

}
