using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace AutoClientSelector;

internal readonly struct IpEditorState
{
    internal readonly Server Server;

    internal IpEditorState(SetIP editor)
    {
        Server = editor?.server;
    }
}

internal static class ClientSelector
{
    internal static void AfterIpEditorAction(
        SetIP editor,
        IpEditorState state)
    {
        var server = state.Server;
        if (server is null || editor is null || editor.isActive)
            return;

        try
        {
            if (string.Equals(
                    server.IP,
                    "0.0.0.0",
                    StringComparison.Ordinal))
            {
                ClearClient(server);
                return;
            }

            SelectMatchingClient(server);
        }
        catch (Exception exception)
        {
            Melon<Mod>.Logger.Warning(
                "Could not select a client after the server IP changed. " +
                $"{exception.GetType().Name}: {exception.Message}");
        }
    }

    private static void ClearClient(Server server)
    {
        var previousClientId = server.GetCustomerID();
        if (previousClientId == -1)
        {
            Melon<Mod>.Logger.Msg(
                $"Server {server.ServerID}: no client was assigned for " +
                "0.0.0.0; current client -1 was left unchanged.");
            return;
        }

        server.UpdateCustomer(-1);
        CoopWorldSync.SyncServerCustomer(server.ServerID, -1);
        Melon<Mod>.Logger.Msg(
            $"Server {server.ServerID}: cleared client {previousClientId} " +
            "for 0.0.0.0.");
    }

    private static void SelectMatchingClient(Server server)
    {
        if (server is null || string.IsNullOrWhiteSpace(server.IP))
            return;

        var matchingClientIds = new HashSet<int>();
        foreach (var customerBase in
                 UnityEngine.Object.FindObjectsOfType<CustomerBase>())
        {
            if (customerBase is null || !customerBase.IsIPPresent(server.IP))
                continue;
            matchingClientIds.Add(customerBase.customerID);
        }

        var currentClientId = server.GetCustomerID();
        if (matchingClientIds.Count == 0)
        {
            Melon<Mod>.Logger.Msg(
                $"Server {server.ServerID}: no matching client was found for " +
                $"{server.IP}; current client {currentClientId} was left " +
                "unchanged.");
            return;
        }

        if (matchingClientIds.Count > 1)
        {
            var matches = string.Join(
                ", ",
                matchingClientIds.OrderBy(clientId => clientId));
            Melon<Mod>.Logger.Warning(
                $"Server {server.ServerID}: multiple clients [{matches}] " +
                $"matched {server.IP}; current client {currentClientId} was " +
                "left unchanged.");
            return;
        }

        var matchingClientId = matchingClientIds.Single();
        if (matchingClientId == currentClientId)
        {
            Melon<Mod>.Logger.Msg(
                $"Server {server.ServerID}: client {matchingClientId} was " +
                $"already selected for {server.IP}.");
            return;
        }

        server.UpdateCustomer(matchingClientId);
        CoopWorldSync.SyncServerCustomer(
            server.ServerID,
            matchingClientId);
        Melon<Mod>.Logger.Msg(
            $"Server {server.ServerID}: changed client from {currentClientId} " +
            $"to {matchingClientId} for {server.IP}.");
    }
}

[HarmonyPatch(typeof(SetIP), nameof(SetIP.ClickButtonOK))]
internal static class ConfirmIpPatch
{
    private static void Prefix(SetIP __instance, out IpEditorState __state)
    {
        __state = new IpEditorState(__instance);
    }

    private static void Postfix(SetIP __instance, IpEditorState __state)
    {
        ClientSelector.AfterIpEditorAction(__instance, __state);
    }
}
