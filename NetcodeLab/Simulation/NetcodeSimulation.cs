namespace NetcodeLab.Simulation;

public sealed record NetworkInput(int Sequence, int MoveX, bool Fire, int ArrivesAt);

public sealed class NetcodeSimulation
{
    private readonly Queue<NetworkInput> _pending = new();
    private int _nextSequence = 1;
    public int TickNumber { get; private set; }
    public int ServerX { get; private set; }
    public double ClientPredictedX { get; private set; }
    public double InterpolatedX { get; private set; }
    public int Reconciliations { get; private set; }
    public double Recoil { get; private set; }
    public int PendingInputs => _pending.Count;

    public void SendInput(int moveX, bool fire, int latencyMilliseconds)
    {
        var latencyTicks = Math.Clamp((int)Math.Round(latencyMilliseconds / 100d), 0, 20);
        ClientPredictedX += moveX;
        if (fire) Recoil = Math.Min(100, Recoil + 8);
        _pending.Enqueue(new(_nextSequence++, moveX, fire, TickNumber + latencyTicks));
    }

    public void AdvanceTick()
    {
        TickNumber++;
        while (_pending.TryPeek(out var input) && input.ArrivesAt <= TickNumber)
        {
            var oldPrediction = ClientPredictedX;
            _pending.Dequeue();
            ServerX += input.MoveX;
            if (input.Fire) Recoil = Math.Min(100, Recoil + 5);
            ClientPredictedX = ServerX + _pending.Sum(i => i.MoveX);
            var correction = ClientPredictedX - oldPrediction;
            if (Math.Abs(correction) > 0.001 || input.Sequence > 0) Reconciliations++;
        }
        InterpolatedX += (ServerX - InterpolatedX) * 0.5;
        Recoil = Math.Max(0, Recoil - 1.5);
    }

    public void Reset()
    {
        _pending.Clear(); _nextSequence = 1; TickNumber = 0; ServerX = 0;
        ClientPredictedX = 0; InterpolatedX = 0; Reconciliations = 0; Recoil = 0;
    }
}
