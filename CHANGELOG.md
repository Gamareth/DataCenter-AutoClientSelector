# Changelog

All notable changes to Auto Client Selector are documented in this file.

## [Unreleased]

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

- Keep automatic selection disabled while a save is loading, preventing server
  reconstruction from triggering client changes before the game is ready.
- Trigger selection from the confirmed IP-editor OK action instead of `Server.SetIP`,
  because server reconstruction continues after the game's nominal load-later
  event.
