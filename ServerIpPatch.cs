using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace AutoClientSelector;

internal readonly struct IpEditorState
{
    internal readonly Server Server;
    internal readonly string PreviousIp;

    internal IpEditorState(SetIP editor)
    {
        Server = editor?.server;
        PreviousIp = Server?.IP ?? string.Empty;
    }
}

internal static class ClientSelector
{
    internal static void AfterIpEditorAction(IpEditorState state)
    {
        var server = state.Server;
        if (server is null)
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

            if (string.Equals(
                    state.PreviousIp,
                    server.IP,
                    StringComparison.Ordinal))
                return;

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
            return;

        server.UpdateCustomer(-1);
        CoopWorldSync.SyncServerCustomer(server.ServerID, -1);
        Melon<Mod>.Logger.Msg(
            $"Server {server.ServerID} IP 0.0.0.0: cleared client " +
            $"{previousClientId}.");
    }

    private static void SelectMatchingClient(Server server)
    {
        if (server is null || string.IsNullOrWhiteSpace(server.IP))
            return;

        int? matchingClientId = null;
        foreach (var customerBase in
                 UnityEngine.Object.FindObjectsOfType<CustomerBase>())
        {
            if (customerBase is null || !customerBase.IsIPPresent(server.IP))
                continue;

            var clientId = customerBase.customerID;
            if (matchingClientId.HasValue && matchingClientId.Value != clientId)
            {
                Melon<Mod>.Logger.Warning(
                    $"Server {server.ServerID} IP {server.IP} matches multiple " +
                    "clients; the current client was left unchanged.");
                return;
            }

            matchingClientId = clientId;
        }

        if (!matchingClientId.HasValue ||
            matchingClientId.Value == server.GetCustomerID())
            return;

        var previousClientId = server.GetCustomerID();
        server.UpdateCustomer(matchingClientId.Value);
        CoopWorldSync.SyncServerCustomer(
            server.ServerID,
            matchingClientId.Value);
        Melon<Mod>.Logger.Msg(
            $"Server {server.ServerID} IP {server.IP}: changed client from " +
            $"{previousClientId} to {matchingClientId.Value}.");
    }
}

[HarmonyPatch(typeof(SetIP), nameof(SetIP.ClickButtonOK))]
internal static class ConfirmIpPatch
{
    private static void Prefix(SetIP __instance, out IpEditorState __state)
    {
        __state = new IpEditorState(__instance);
    }

    private static void Postfix(IpEditorState __state)
    {
        ClientSelector.AfterIpEditorAction(__state);
    }
}
