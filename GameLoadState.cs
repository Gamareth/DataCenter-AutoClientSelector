using Il2Cpp;
using MelonLoader;

namespace AutoClientSelector;

internal static class GameLoadState
{
    private static SaveSystem.OnLoadingData _loadingData;
    private static SaveSystem.OnLoadingDataLater _loadingDataLater;
    private static bool _initialized;

    internal static bool IsReady { get; private set; }

    internal static void Initialize()
    {
        if (_initialized)
            return;

        IsReady = false;
        _loadingData = (Action)OnLoadingData;
        _loadingDataLater = (Action)OnLoadingDataLater;
        SaveSystem.onLoadingData += _loadingData;
        SaveSystem.onLoadingDataLater += _loadingDataLater;
        _initialized = true;
    }

    private static void OnLoadingData()
    {
        IsReady = false;
        Melon<Mod>.Logger.Msg(
            "Save loading started; automatic client selection is suspended.");
    }

    private static void OnLoadingDataLater()
    {
        IsReady = true;
        Melon<Mod>.Logger.Msg(
            "Save loading completed; automatic client selection is active.");
    }
}
