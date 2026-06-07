using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public enum GameSpeed
{
    Paused = 0,
    Slow = 2000,
    Normal = 1000,  // 1 second per tick
    Fast = 250,     // 250ms per tick
    SuperFast = 0   // Run next tick immediately after finishing
}

public class ServerTickController
{
    //private readonly SimulationEngine _engine;
    private GameSpeed _currentSpeed = GameSpeed.Normal;
    private CancellationTokenSource _cts;
    private Task _loopTask;

    //public ServerTickController(SimulationEngine engine)
    //{
    //    _engine = engine;
    //}

    public void SetSpeed(GameSpeed speed)
    {
        _currentSpeed = speed;
        Console.WriteLine($"[SERVER] Speed changed to: {speed}");
    }

    public void Start()
    {
        _cts = new CancellationTokenSource();
        _loopTask = Task.Run(() => RunLoopAsync(_cts.Token));
    }

    public async Task StopAsync()
    {
        if (_cts == null) return;
        _cts.Cancel();
        try { await _loopTask; } catch (OperationCanceledException) { }
    }

    private async Task RunLoopAsync(CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = new Stopwatch();

        while (!cancellationToken.IsCancellationRequested)
        {
            // If paused, yield control and check again shortly
            if (_currentSpeed == GameSpeed.Paused)
            {
                await Task.Delay(100, cancellationToken);
                continue;
            }

            long targetDurationMs = (long)_currentSpeed;
            stopwatch.Restart();

            try
            {
                // 1. Process incoming player commands accumulated during the last tick
                // _engine.ProcessIncomingCommands();

                // 2. Execute the simulation tick
                // _engine.DoTick();

                // 3. Broadcast the state/changes to network clients
                // BroadcastTickToNetwork(_engine.CurrentTick);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CRITICAL] Error during tick: {ex.Message}");
                // In production, consider pausing the game here so players don't desync further
                _currentSpeed = GameSpeed.Paused;
            }

            stopwatch.Stop();
            long elapsedMs = stopwatch.ElapsedMilliseconds;

            if (elapsedMs > targetDurationMs && targetDurationMs > 0)
            {
                //Console.WriteLine($"[WARN] Tick {_engine.CurrentTick} lagged! Took {elapsedMs}ms, Max target: {targetDurationMs}ms.");
            }

            // Calculate remaining sleep time to maintain steady rhythm
            long remainingSleepMs = targetDurationMs - elapsedMs;

            if (remainingSleepMs > 0)
            {
                // Wait out the remainder of the tick duration
                await Task.Delay((int)remainingSleepMs, cancellationToken);
            }
            else
            {
                // If remainingSleepMs <= 0 (either SuperFast mode or server is lagging),
                // we yield execution momentarily so the CPU isn't completely choked, 
                // then jump straight into the next tick.
                await Task.Yield();
            }
        }
    }
}