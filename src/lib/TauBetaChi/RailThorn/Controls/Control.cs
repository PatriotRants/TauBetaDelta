using OpenTK.Mathematics;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.RailThorn.Controls;

public abstract class Control
{
    protected static readonly LoggerManager LOGGER = LoggerManager.Instance;

    public string Name { get; }
    public Vector2i Location { get; set; } = (0, 0);

    protected Control(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Control initialization
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// Update
    /// </summary>
    public virtual void Update() { }
    /// <summary>
    /// Render
    /// </summary>
    public virtual void Render() { }
}