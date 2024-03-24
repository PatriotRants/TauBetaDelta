using ForgeWorks.RailThorn.Graphics;

namespace ForgeWorks.RailThorn.Controls;

public class Label : TextRenderer
{
    private const string DEFAULT_NAME = $"{nameof(Label)}_";
    private static int SPIN_COUNT = 0;

    public Label(IViewContainer container) : this(container, $"{DEFAULT_NAME}{SPIN_COUNT++:###}") { }
    public Label(IViewContainer container, string name) : base(container, name) { }
}
