namespace ForgeWorks.GlowFork.Tasking;

/// <summary>
/// Queue implementation for <see cref="GameTask"/> items with it's own threaded runners.
/// 
/// <para>
/// Enqueue all tasks prior to calling Start. Once started, no new items may be enqueued. The Queue will be empty.
/// </para>
/// <para>
/// TODO: any faulted tasks need to be logged; maybe a flag on ITaskItem to stop queue if fault
/// </para>
/// </summary>
public sealed class TaskQueue
{
    private readonly Queue<ITaskItem> _taskQueue = new();

    /* **
        Task runners - initialized with a set number of runner agents
    ** */
    private readonly Task[] _taskAgents;

    public TaskQueueStatus Status { get; private set; }

    /// <summary>
    /// Construct a new Task Queue with a specified number of Task Agents
    /// </summary>
    public TaskQueue(int agentCount)
    {
        //  create readonly array of Task
        _taskAgents = Enumerable
            .Repeat(new Task(() => { }), agentCount)
            .ToArray();

        //  re-assign Task execution
        var i = 0;
        while (i < _taskAgents.Length)
        {
            _taskAgents[i] = new Task(() =>
            {
                //  loop while dequeue a task item
                while (_taskQueue.TryDequeue(out ITaskItem task))
                {
                    task.Execute();
                    Thread.SpinWait(1000);
                }
            });

            ++i;
        }
    }

    /// <summary>
    /// Adds an <see cref="ITaskItem" /> to the end of the queue.
    /// </summary>
    public void Enqueue(ITaskItem taskItem)
    {
        if (Status != TaskQueueStatus.Executing)
        {
            _taskQueue.Enqueue(taskItem);
        }
    }
    /// <summary>
    /// Starts task queue execution
    /// </summary>
    public void Start(ManualResetEvent gateEvent, bool asSync = false)
    {
        foreach (var agent in _taskAgents)
        {
            agent.Start();

            if (asSync)
            {
                agent.Wait();
            }
        }

        gateEvent.Set();
    }
}

public enum TaskQueueStatus
{
    Unknown = 0,
    Idle = 1,
    Executing = 2,
}
