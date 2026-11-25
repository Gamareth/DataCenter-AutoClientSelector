# Changelog

All notable changes to Auto Client Selector are documented in this file.

## [1.0.1] - 2026-11-25

### Fixed

- Detect IP assignments made through the game's server IP setter, including
  assignments made by DHCP Mod without opening the IP editor.
- Run client selection after the save finishes loading and only for servers
  whose IP setter was called.

## [1.0.0] - 2026-08-09

### Added

- Add verified automatic Steam Workshop updates after the initial manual
  installation.
- Validate Workshop subscription state, assembly identity, version, timestamp,
  and SHA-256 before staging an update.
- Install a verified update only after Data Center exits and activate it on the
  next launch.
- Support newer same-version compatibility payloads by tracking the applied
  Workshop timestamp.
- Add `AutoUpdateFromWorkshop`, enabled by default, to
  `UserData/AutoClientSelector.cfg`.
- Keep a persistent updater audit log and remove temporary updater files after
  a successful update.

### Changed

- Describe IP matching in terms of Data Center's existing client-matching logic
  without imposing undocumented network limitations.

## [0.1.6-rc.1] - 2026-08-08

### Added

- Automatically select a unique matching client after a server IP is saved.
- Recognize client subnets and custom subnets registered through a direct route
  to a client subnet by using the game's customer-base network registration.
- Leave the current client unchanged when no client or multiple clients match.
- Synchronize an automatic client change through the game's co-op sync API.
- Clear the client assignment when `0.0.0.0` is confirmed in the IP editor.
- Log whether a confirmed IP found no client, matched the already selected
  client, changed the client, or matched multiple clients.
- Keep match-result messages factual without claiming which network mechanism
  produced the match.
- Recheck every IP accepted with the editor's OK button, including unchanged
  valid IPs, while ignoring rejected confirmations.
- Include the current client ID in no-change logs and list all unique client
  IDs when multiple clients match.

### Fixed

- Trigger selection only from the confirmed IP-editor OK action instead of
  `Server.SetIP`, preventing save reconstruction from invoking the mod.
