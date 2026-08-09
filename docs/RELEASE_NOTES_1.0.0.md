# Auto Client Selector 1.0.0

Auto Client Selector automatically selects the matching client after a valid
server IP is confirmed, using Data Center's existing client-matching logic.

## Highlights

- Automatically select one unique matching client.
- Clear the assigned client and logo by confirming `0.0.0.0`.
- Leave the current client unchanged when no client or multiple clients match.
- Receive verified Steam Workshop updates after the initial manual
  installation.
- Disable automatic Workshop updates with `AutoUpdateFromWorkshop = false` in
  `UserData/AutoClientSelector.cfg`.

The first installation remains manual because Data Center does not load code
mod DLLs directly from the Workshop content directory. GitHub users who do not
subscribe to the Workshop continue to update manually.
