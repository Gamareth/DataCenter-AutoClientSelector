# Changelog

All notable changes to Auto Client Selector are documented in this file.

## [Unreleased]

### Added

- Automatically select a unique matching client after a server IP is saved.
- Recognize client subnets and custom subnets registered through a direct route
  to a client subnet by using the game's customer-base network registration.
- Leave the current client unchanged when no client or multiple clients match.
- Synchronize an automatic client change through the game's co-op sync API.

### Fixed

- Keep automatic selection disabled while a save is loading, preventing server
  reconstruction from triggering client changes before the game is ready.
- Trigger selection from the confirmed IP-editor OK action instead of `Server.SetIP`,
  because server reconstruction continues after the game's nominal load-later
  event.
