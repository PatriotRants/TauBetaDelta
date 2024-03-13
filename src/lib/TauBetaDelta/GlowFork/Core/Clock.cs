namespace ForgeWorks.GlowFork;

public class Clock
{
    //  tick interval in ms
    const byte INTERVAL = 20;

    private static readonly Lazy<Clock> _clock = new(() => new());

    private Timer _timer;
    private DateTime _startTime;

    public static Clock Instance => _clock.Value;

    public TimeSpan Interval { get; }

    public event Action<TimeSpan, DateTime> Tick;

    private Clock()
    {
        Interval = TimeSpan.FromMilliseconds(INTERVAL);
    }

    public static void Start()
    {
        Instance.StartClock();
    }


    private void StartClock()
    {
        _startTime = DateTime.UtcNow;
        _timer = new Timer(OnTimerSet, _startTime, 0, Interval.Milliseconds);
    }

    private void OnTimerSet(object state)
    {
        var utcNow = DateTime.UtcNow;
        var timeSpan = utcNow - ((DateTime)state);

        Tick?.Invoke(timeSpan, utcNow);
    }
}
