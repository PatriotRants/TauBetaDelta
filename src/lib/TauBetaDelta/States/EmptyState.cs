namespace ForgeWorks.TauBetaDelta;

public abstract partial class GameState
{
    public static GameState Empty { get; } = new EmptyState();

    private class EmptyState : GameState
    {
        internal EmptyState()
        {
            Name = "Empty";
            View = null;
        }

        public override void Init() { }

        public override void Dispose() { }
    }
}
