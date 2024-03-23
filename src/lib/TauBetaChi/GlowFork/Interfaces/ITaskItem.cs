using ForgeWorks.ShowBird.Messaging;

namespace ForgeWorks.GlowFork.Tasking;


public delegate string TaskAgent(UpdateAgent updateAgent);

public interface ITaskItem : IUpdate
{
    /// <summary>
    /// Execute the current task item
    /// </summary>
    void Execute();
}
