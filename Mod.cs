using MelonLoader;

[assembly: MelonInfo(
    typeof(AutoClientSelector.Mod),
    AutoClientSelector.Mod.DisplayName,
    AutoClientSelector.Mod.Version,
    AutoClientSelector.Mod.Author)]
[assembly: MelonGame("Waseku", "Data Center")]

namespace AutoClientSelector;

public sealed class Mod : MelonMod
{
    internal const string DisplayName = "Auto Client Selector";
    internal const string Version = "0.1.4";
    internal const string Author = "Gamareth";

    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg(
            $"{DisplayName} {Version} initialized. " +
            "Automatic selection runs only after an IP editor action changes " +
            "a server IP.");
    }
}
