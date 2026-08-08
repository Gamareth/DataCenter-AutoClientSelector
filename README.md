# Auto Client Selector

Auto Client Selector is a lightweight MelonLoader mod for Data Center.

After a server IP address is saved, the mod asks each active customer base
whether the IP belongs to it. This uses the client subnets and custom subnets
that the game registers through direct routes to client subnets. If exactly one
unique client matches, the mod updates the server to that client. If no client
or multiple clients match, the existing client remains unchanged.

Confirming `0.0.0.0` clears the current client assignment and client logo.
Confirming an unchanged valid IP deliberately runs the selection again. An IP
rejected by the game's editor does not trigger selection.

Indirect chains through multiple custom subnets are outside the supported V1
scope.

The mod adds no buttons, shortcuts, settings, or automatic update mechanism.
It is provided as-is; compatibility updates are not guaranteed.

## Build

Requirements:

- .NET 6 SDK or newer;
- Data Center with MelonLoader installed;
- generated IL2CPP assemblies under `MelonLoader/Il2CppAssemblies`.

Build from PowerShell:

```powershell
dotnet build AutoClientSelector.csproj -c Release `
  /p:DataCenterGamePath='D:\SteamLibrary\steamapps\common\Data Center'
```

Copy `bin/Release/net6.0/AutoClientSelector.dll` to the game's `Mods` folder
while the game is not running.

## Validation

Use a backed-up test save and verify all of these cases:

1. An IP belonging to one customer selects that customer.
2. An IP with no matching customer leaves the current customer unchanged.
3. An IP in a custom subnet with a direct route to a client subnet selects that
   client.
4. Removing the direct route causes the custom-subnet IP to leave the current
   client unchanged.
5. Loading an existing save does not unexpectedly change server clients.
6. In co-op, the selected client remains synchronized for all players.

See [release testing](docs/RELEASE_TESTING.md) for the complete release gate.
