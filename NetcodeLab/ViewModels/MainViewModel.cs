using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using NetcodeLab.Simulation;

namespace NetcodeLab.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly NetcodeSimulation _simulation = new();
    [ObservableProperty] private int _latencyMs = 200;
    [ObservableProperty] private string _status = "Envie uma ação e avance o tempo simulado para observar a chegada ao servidor.";
    [ObservableProperty] private string _snapshot = "Cliente previsto: 0 · servidor: 0 · interpolado: 0";
    [ObservableProperty] private string _networkStats = "Latência: 200 ms · pendentes: 0 · reconciliações: 0 · recoil: 0°";
    [ObservableProperty] private int _serverPosition;

    [RelayCommand]
    private void SendMoveAndShot()
    {
        _simulation.SendInput(1, true, Math.Clamp(LatencyMs, 0, 2000));
        Refresh("Cliente aplicou prediction imediatamente; movimento e disparo aguardam o servidor.");
    }

    [RelayCommand]
    private void AdvanceTick()
    {
        _simulation.AdvanceTick();
        Refresh("Tick de 100 ms: servidor processa entradas vencidas e cliente interpola snapshots.");
    }

    [RelayCommand]
    private void Reset()
    {
        _simulation.Reset(); Refresh("Simulação reiniciada.");
    }

    private void Refresh(string message)
    {
        Status = message;
        ServerPosition = Math.Clamp(_simulation.ServerX, 0, 20);
        Snapshot = $"Cliente previsto: {_simulation.ClientPredictedX:F0} · servidor: {_simulation.ServerX} · interpolado: {_simulation.InterpolatedX:F1}";
        NetworkStats = $"Tick: {_simulation.TickNumber} · latência: {Math.Clamp(LatencyMs, 0, 2000)} ms · pendentes: {_simulation.PendingInputs} · reconciliações: {_simulation.Reconciliations} · recoil: {_simulation.Recoil:F1}°";
    }
}
