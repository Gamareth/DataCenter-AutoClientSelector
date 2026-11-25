using System.Net;
using System.Net.Sockets;
using Il2Cpp;
using MelonLoader;

namespace AutoClientSelector;

internal static class ClientSelector
{
    internal static void Apply(Server server, string ip)
    {
        try
        {
            if (server == null ||
                !string.Equals(server.IP, ip, StringComparison.Ordinal) ||
                !IPAddress.TryParse(ip, out var address) ||
                address.AddressFamily != AddressFamily.InterNetwork)
            {
                return;
            }

            if (ip == "0.0.0.0")
                ClearClient(server);
            else
                SelectMatchingClient(server, ip);
        }
        catch (Exception exception)
        {
            Melon<Mod>.Logger.Warning(
                "Could not select a client after the server IP was set. " +
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

    private static void SelectMatchingClient(Server server, string ip)
    {
        var matchingClientIds = new HashSet<int>();
        foreach (var customerBase in
                 UnityEngine.Object.FindObjectsOfType<CustomerBase>())
        {
            if (customerBase is null || !customerBase.IsIPPresent(ip))
                continue;
            matchingClientIds.Add(customerBase.customerID);
        }

        var currentClientId = server.GetCustomerID();
        if (matchingClientIds.Count == 0)
        {
            Melon<Mod>.Logger.Msg(
                $"Server {server.ServerID}: no matching client was found for " +
                $"{ip}; current client {currentClientId} was left " +
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
                $"matched {ip}; current client {currentClientId} was " +
                "left unchanged.");
            return;
        }

        var matchingClientId = matchingClientIds.Single();
        if (matchingClientId == currentClientId)
        {
            Melon<Mod>.Logger.Msg(
                $"Server {server.ServerID}: client {matchingClientId} was " +
                $"already selected for {ip}.");
            return;
        }

        server.UpdateCustomer(matchingClientId);
        CoopWorldSync.SyncServerCustomer(
            server.ServerID,
            matchingClientId);
        Melon<Mod>.Logger.Msg(
            $"Server {server.ServerID}: changed client from {currentClientId} " +
            $"to {matchingClientId} for {ip}.");
    }
}
