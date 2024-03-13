namespace ForgeWorks.GlowFork.Automata;

/// <summary>
/// Base State
/// </summary>
public abstract class State
{
    public string Name { get; protected set; }
    public string Next { get; protected set; }
}
