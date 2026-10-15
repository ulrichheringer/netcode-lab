using NetcodeLab.Simulation;

var sim = new NetcodeSimulation(); sim.SendInput(1, true, 200);
if (sim.ClientPredictedX != 1 || sim.ServerX != 0 || sim.Recoil <= 0) throw new Exception("Local prediction or recoil failed.");
sim.AdvanceTick(); if (sim.ServerX != 0) throw new Exception("Input arrived before its latency expired.");
sim.AdvanceTick(); if (sim.ServerX != 1 || sim.Reconciliations != 1 || sim.InterpolatedX <= 0) throw new Exception("Server update or reconciliation failed.");
Console.WriteLine("Netcode checks passed.");
