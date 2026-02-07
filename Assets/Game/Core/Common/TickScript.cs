
using System.Collections;
using System.Diagnostics;


public class TickScript
{
    const double TickDuration = 1.0 / 60.0;
    bool isPaused = true;
    Stopwatch stopwatch;
    double lastTime;
    double accumulatedTime;
    long tick;

    public TickScript()
    {
        stopwatch = Stopwatch.StartNew();
        lastTime = stopwatch.Elapsed.TotalSeconds;
        accumulatedTime = 0.0;
        tick = 0;
    }

    public void Update()
    {
        double currentTime = stopwatch.Elapsed.TotalSeconds;
        double deltaTime = currentTime - lastTime;
        lastTime = currentTime;

        accumulatedTime += deltaTime;

        while (accumulatedTime >= TickDuration)
        {
            SimulateTick(tick);
            tick++;
            accumulatedTime -= TickDuration;
        }
    }

    void SimulateTick(long tick)
    {
        // deterministic simulation here
    }
public void PauseGame()
    {
        isPaused = true;
    }
    public void OneSpeed()
    {
        isPaused = false;
        TickDuration = 5f;
    }
    public void TwoSpeed()
    {
        isPaused = false;
        TickDuration =3f;
    }
    public void ThreeSpeed()
    {
        isPaused = false;
        TickDuration = 1f;
    }
}}