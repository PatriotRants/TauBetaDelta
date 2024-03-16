namespace ForgeWorks.GlowFork.Automata;

/// <summary>
/// Base State
/// </summary>
public abstract class State
{
    /// <summary>
    /// Get the current <see cref="Name"/>
    /// </summary>
    public string Name { get; protected set; }
    /// <summary>
    /// Get the <see cref="Next"/> valid <see cref="State"/> to
    /// which this <see cref="State"/> may transition
    /// </summary>
    public string Next { get; protected set; }
}
