# netcode-lab

Explore multiplayer netcode with a local client and server simulation. Adjust latency, send inputs, and watch prediction, interpolation, reconciliation, and recoil.

## Run

Requires the .NET 10 SDK. Browser builds also need the WebAssembly workload: `dotnet workload install wasm-tools`.

### Desktop

~~~sh
dotnet run --project NetcodeLab.Desktop/NetcodeLab.Desktop.csproj
~~~

### Browser

~~~sh
dotnet run --project NetcodeLab.Browser/NetcodeLab.Browser.csproj
~~~

## Example

Set latency to 200 ms, send an input, and advance a few ticks.
