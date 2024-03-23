using OpenTK.Mathematics;

using ForgeWorks.GlowFork.Fonts;

namespace ForgeWorks.RailThorn.Controls;

public class Label : Control
{
    private const string DEFAULT_NAME = $"{nameof(Label)}_";
    private static int SPIN_COUNT = 0;

    public string Text { get; set; } = string.Empty;
    public Color4 Color { get; set; } = Color4.Black;
    public Font Font { get; set; } = "LiberationMono-Regular"; //   default font

    public Label() : this($"{DEFAULT_NAME}{SPIN_COUNT++:###}") { }
    public Label(string name) : base(name) { }
}
