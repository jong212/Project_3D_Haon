using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : Singleton<RelayManager>
{
    private bool isHost = false;
    public string joinCode;
    private string ip;
    private int port;
    private byte[] key;
    private byte[] connectionData;
    private byte[] hostConnectionData;
    private System.Guid allocationId;
    private byte[] allocationIdBytes;

    public bool IsHost
    {
        get { return isHost; }
    }

    public async Task<string> CreateRelay(int maxConnections)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("CreateRelay: joinCode = " + joinCode);

            RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
            ip = dtlsEndpoint.Host;
            port = dtlsEndpoint.Port;

            allocationId = allocation.AllocationId;
            allocationIdBytes = allocation.AllocationIdBytes;
            connectionData = allocation.ConnectionData;
            key = allocation.Key;

            isHost = true;
            return joinCode;
        }
        catch (System.Exception ex)
        {
            Debug.Log($"Failed to create relay: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> JoinRelay(string joinCode)
    {
        if (string.IsNullOrEmpty(joinCode))
        {
            Debug.LogError("JoinRelay: joinCode is null or empty.");
            return false;
        }

        try
        {
            Debug.Log("JoinRelay: Attempting to join relay with joinCode: " + joinCode);
            this.joinCode = joinCode;
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            if (allocation == null)
            {
                Debug.LogError("JoinRelay: Allocation is null.");
                return false;
            }

            RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.FirstOrDefault(conn => conn.ConnectionType == "dtls");
            if (dtlsEndpoint == null)
            {
                Debug.LogError("JoinRelay: No DTLS endpoint found.");
                return false;
            }

            ip = dtlsEndpoint.Host;
            port = dtlsEndpoint.Port;

            allocationId = allocation.AllocationId;
            allocationIdBytes = allocation.AllocationIdBytes;
            connectionData = allocation.ConnectionData;
            hostConnectionData = allocation.HostConnectionData;
            key = allocation.Key;

            Debug.Log("JoinRelay: Successfully joined the relay server.");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"JoinRelay failed: {ex.Message}");
            return false;
        }
    }

    public string GetAllocationId()
    {
        return allocationId.ToString();
    }

    public string GetConnectionData()
    {
        return connectionData.ToString();
    }

    public (byte[] AllocationId, byte[] Key, byte[] ConnectionData, string dtlsAddress, int dtlsPort) GetHostConnectionInfo()
    {
        return (allocationIdBytes, key, connectionData, ip, port);
    }

    public (byte[] AllocationId, byte[] Key, byte[] ConnectionData, byte[] HostConnectionData, string dtlsAddress, int dtlsPort) GetClientConnectionInfo()
    {
        return (allocationIdBytes, key, connectionData, hostConnectionData, ip, port);
    }
}
