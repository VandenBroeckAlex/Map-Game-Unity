using System;
using System.Diagnostics;

public class TickSystem
{
    public event Action OnTick;

    public long Tick { get; private set; }

    private readonly Stopwatch stopwatch = new Stopwatch();

    private double tickDuration = 1.0 / 60.0;
    private double accumulatedTime;
    private double lastTime;
    private double timeScale = 1.0;
    private bool isPaused = true;

    public TickSystem(double tickRate = 60.0)
    {
        tickDuration = 1.0 / tickRate;
    }

    public void Start()
    {
        isPaused = false;
        accumulatedTime = 0;
        lastTime = 0;
        stopwatch.Restart();
    }

    public void Pause()
    {
        isPaused = true;
        stopwatch.Stop();
    }

    public void SetSpeed(double scale)
    {
        timeScale = scale;
    }

    public void Update()
    {
        if (isPaused)
            return;

        double currentTime = stopwatch.Elapsed.TotalSeconds;
        double deltaTime = (currentTime - lastTime) * timeScale;
        lastTime = currentTime;

        accumulatedTime += deltaTime;

        while (accumulatedTime >= tickDuration)
        {
            Tick++;
            OnTick?.Invoke();
            accumulatedTime -= tickDuration;
        }
    }
}
