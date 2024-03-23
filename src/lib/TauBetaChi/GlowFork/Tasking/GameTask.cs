using ForgeWorks.ShowBird.Messaging;

namespace ForgeWorks.GlowFork.Tasking;

/// <summary>
/// A Game Task <see cref="ITaskItem"/> that pushes status updates to the assigned Update delegate
/// </summary>
public class GameTask : ITaskItem
{

    private TaskAgent Task { get; }

    /// <summary>
    /// Get or set the <inheritdoc cref="IUpdate.Updater" />
    /// </summary>
    public UpdateAgent Updater { get; set; }

    /// <summary>
    /// Construct a new Game Task item with a Task Agent (delegate)
    /// </summary>
    /// <param name="taskAgent"></param>
    public GameTask(TaskAgent taskAgent)
    {
        Task = taskAgent;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public void Execute()
    {
        Updater?.Invoke(Task(Updater));
    }
}