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
    internal const string Version = "1.0.0";
    internal const string Author = "Gamareth";

    public override void OnInitializeMelon()
    {
        ModSettings.Initialize();
        WorkshopAutoUpdater.CleanupAfterSuccessfulUpdate();
        var workshopUpdateState = ModSettings.AutoUpdateFromWorkshop
            ? "enabled for subscribers"
            : "disabled";
        LoggerInstance.Msg(
            $"{DisplayName} {Version} initialized. " +
            "Automatic selection runs after a valid server IP is confirmed; " +
            $"Workshop auto-update: {workshopUpdateState}.");
    }

    public override void OnLateInitializeMelon()
    {
        WorkshopAutoUpdater.CheckAfterInitialization();
    }

    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        WorkshopAutoUpdater.CheckAfterInitialization();
    }

    public override void OnApplicationQuit()
    {
        WorkshopAutoUpdater.OnApplicationQuit();
    }
}
