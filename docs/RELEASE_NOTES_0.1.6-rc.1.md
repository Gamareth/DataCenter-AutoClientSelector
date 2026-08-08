# Auto Client Selector 0.1.6-rc.1

This is the first local release candidate for Auto Client Selector.

## Behaviour

- Rechecks the client whenever the game accepts the IP editor's OK action,
  including when the valid IP is unchanged.
- Selects a client only when exactly one unique client matches.
- Leaves the existing client unchanged when no client or multiple clients
  match.
- Clears the assigned client when `0.0.0.0` is confirmed.
- Ignores IP changes performed internally while a save is reconstructed.
- Produces concise outcome logs containing the relevant IP and client IDs.

## Validation completed

- Release build succeeds against the installed Data Center IL2CPP assemblies.
- Save reconstruction no longer invokes automatic client selection.
- Confirmed IP changes can select a unique client.
- Confirming `0.0.0.0` clears an assigned client.
- The installed test DLL and local Release build were hash-verified.
- A slow Quit to Desktop issue was reproduced with zero mods loaded and is
  therefore tracked separately as a Data Center or MelonLoader shutdown issue.

## Required before public v1.0.0

- Verify an unchanged valid IP confirmed with OK triggers a recheck.
- Verify rejected invalid and duplicate IPs do not trigger a result log.
- Verify a direct client-subnet match.
- Verify a custom subnet with a direct route to a client subnet.
- Verify no-match, same-client, and multiple-client result logs.
- Decide whether co-op remains explicitly unsupported for v1.0.0.
