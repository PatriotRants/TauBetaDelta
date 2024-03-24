using ForgeWorks.GlowFork.Logging;
using ForgeWorks.RailThorn.Graphics;

namespace ForgeWorks.RailThorn.Controls;

public abstract class RendererControl : Control
{
    protected static readonly ShaderManager SHADERS = ShaderManager.Instance;
    protected static readonly AssetManager ASSETS = AssetManager.Instance;
    protected static readonly GLLogger GLLOGGER = GLLogger.Instance;

    protected Shader shader { get; init; }
    protected Texture texture { get; init; }

    protected RendererControl(string name) : base(name) { }
}
