using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace AutoClientSelector;

internal static class ServerIpUpdates
{
    private static Dictionary<int, (Server Server, string Ip)> _pending = new();
    private static Dictionary<int, (Server Server, string Ip)> _batch = new();

    internal static void Record(Server server, string ip)
    {
        try
        {
            if (server != null && MaySelectClient())
                _pending[server.GetInstanceID()] = (server, ip);
        }
        catch (Exception exception)
        {
            LogError(exception);
        }
    }

    internal static void DiscardPending() => _pending.Clear();

    internal static void ProcessPending()
    {
        if (_pending.Count == 0)
            return;

        // Let the IP writer finish its operation before changing the client.
        (_pending, _batch) = (_batch, _pending);
        try
        {
            if (!MaySelectClient())
                return;

            foreach (var (server, ip) in _batch.Values)
                ClientSelector.Apply(server, ip);
        }
        catch (Exception exception)
        {
            LogError(exception);
        }
        finally
        {
            _batch.Clear();
        }
    }

    private static bool MaySelectClient()
    {
        var network = WaypointInitializationSystem.Instance;
        return network != null && NetworkMap.instance != null &&
            !network.isLoading && network.HasCompletedPostLoadEvaluation &&
            !CoopWorldSync.IsApplying && !CoopWorldSync.IsApplyingRemoval &&
            !CoopWorldSync.IsCoopClient &&
            (!CoopWorldSync.CoopActive || CoopWorldSync.IsHost);
    }

    private static void LogError(Exception exception)
    {
        Melon<Mod>.Logger.Warning(
            "Could not process a server IP assignment. " +
            $"{exception.GetType().Name}: {exception.Message}");
    }
}

[HarmonyPatch(typeof(Server), nameof(Server.SetIP), typeof(string))]
internal static class SetServerIpPatch
{
    private static void Postfix(Server __instance, string __0)
    {
        ServerIpUpdates.Record(__instance, __0);
    }
}

[HarmonyPatch(
    typeof(WaypointInitializationSystem),
    nameof(WaypointInitializationSystem.LoadNetworkState))]
internal static class NetworkLoadPatch
{
    private static void Prefix()
    {
        ServerIpUpdates.DiscardPending();
    }
}
