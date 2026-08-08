# Release testing

Use a backed-up save. Do not publish a release until every required test passes
with the exact DLL intended for publication.

## Installation and startup

- [ ] Data Center is closed before replacing the DLL.
- [ ] Only one copy of `AutoClientSelector.dll` is present in `Mods`.
- [ ] The MelonLoader log reports `Auto Client Selector` with the expected
      version and no patching errors.
- [ ] The log reports that selection runs only after a confirmed IP-editor action.

## Required matching scenarios

- [ ] Saving a server IP inside a client subnet selects that client.
- [ ] Saving a server IP inside a custom subnet with one direct route to a
      client subnet selects that client.
- [ ] Removing or disabling that direct route causes the same custom-subnet IP
      to leave the existing client unchanged.
- [ ] Saving an IP with no matching client leaves the existing client unchanged.
- [ ] A match involving more than one client leaves the existing client
      unchanged and writes a warning to the MelonLoader log.
- [ ] Saving an IP that already belongs to the selected client causes no visible
      change.

## Regression checks

- [ ] Loading an existing save does not unexpectedly change server clients.
- [ ] No automatic client-change messages appear while servers are reconstructed.
- [ ] Confirming an edited IP with the OK button triggers matching.
- [ ] Using Next IP without confirming does not trigger matching.
- [ ] Confirming `0.0.0.0` clears an assigned client and its logo.
- [ ] Confirming `0.0.0.0` on an unassigned server causes no warning or error.
- [ ] A changed IP with no match logs that the current client was unchanged.
- [ ] A unique match logs whether the client changed or was already selected.
- [ ] Match logs describe the source as a client subnet or directly routed
      custom subnet without claiming an unverified exact route type.
- [ ] Manual previous/next client controls still work.
- [ ] Saving a new IP still performs the game's duplicate-IP validation.
- [ ] The selected client persists after saving and reloading.
- [ ] No recurring exceptions appear in the MelonLoader log.

## Co-op release gate

Do not claim co-op support until these checks have passed with two players:

- [ ] The host and client see the same automatic client selection.
- [ ] Saving and reloading preserves the synchronized selection.
