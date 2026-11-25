# Auto Client Selector

Auto Client Selector is a lightweight MelonLoader mod for Data Center.

After a server IP address is saved, the mod asks each active customer base
whether the IP belongs to it, using Data Center's existing client-matching
logic. If exactly one unique client matches, the mod updates the server to that
client. If no client or multiple clients match, the existing client remains
unchanged.

After the save is loaded, selection runs for the affected server whenever
Data Center's `Server.SetIP` method is called. This includes the game's IP
editor and mods that use the same method, such as DHCP Mod. Setting
`0.0.0.0` clears the current client assignment and logo. Confirming the same
valid IP rechecks the current assignment when the game saves it again.

The mod adds no buttons or shortcuts. Subscribed Workshop users receive
verified automatic updates after the initial manual installation. Set
`AutoUpdateFromWorkshop = false` in
`UserData/AutoClientSelector.cfg` to keep manual control.

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

1. An IP matched to one customer by the game selects that customer.
2. An IP with no matching customer leaves the current customer unchanged.
3. An IP matched to multiple customers leaves the current customer unchanged.
4. Loading an existing save does not unexpectedly change server clients.
5. A subscribed hidden test update is installed only after Data Center exits.
6. In co-op, the selected client remains synchronized for all players.

See [release testing](docs/RELEASE_TESTING.md) for the complete release gate.
