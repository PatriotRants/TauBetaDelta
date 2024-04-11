using ForgeWorks.RailThorn.Logging;
using ForgeWorks.RailThorn.Graphics;

namespace ForgeWorks.RailThorn.Controls;

public abstract class RendererControl : Control
{
    protected static readonly ShaderManager SHADERS = ShaderManager.Instance;
    protected static readonly AssetManager ASSETS = AssetManager.Instance;
    protected static readonly GLLogger GLLOGGER = GLLogger.Instance;

    protected Shader Shader { get; init; }

    protected RendererControl(string name) : base(name) { }

    /// <summary>
    /// Update
    /// </summary>
    public virtual void Update() { }
    /// <summary>
    /// Render
    /// </summary>
    public virtual void Render() { }
}
