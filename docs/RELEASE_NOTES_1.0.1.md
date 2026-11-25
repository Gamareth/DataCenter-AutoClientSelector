# Auto Client Selector 1.0.1

Auto Client Selector now checks for a matching client when Data Center sets a
server's IP address. DHCP Mod by Bloody can trigger client selection without
opening the game's IP editor.

Selection runs after the save is loaded. One matching client is selected
automatically. The current client is kept when there is no match or more than
one match, and setting `0.0.0.0` clears the assignment and logo.
