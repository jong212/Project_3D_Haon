using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class RelayManager : Singleton<RelayManager>
{
    private bool isHost = false;


    private string joinCode;
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

    public async Task<string> CreateRelay(int maxConnection)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        RelayServerEndpoint dtlsEnpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
        ip = dtlsEnpoint.Host;
        port = dtlsEnpoint.Port;

        allocationId = allocation.AllocationId;
        allocationIdBytes = allocation.AllocationIdBytes;
        connectionData = allocation.ConnectionData;
        key = allocation.Key;

        isHost = true;

        return joinCode;

    }

    public async Task<bool> JoinRelay(string joinCode)
    {
        this.joinCode = joinCode;
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        RelayServerEndpoint dtlsEnpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
        ip = dtlsEnpoint.Host;
        port = dtlsEnpoint.Port;

        allocationId = allocation.AllocationId;
        allocationIdBytes = allocation.AllocationIdBytes;
        connectionData = allocation.ConnectionData;
        hostConnectionData = allocation.HostConnectionData;
        key = allocation.Key;

        return true;
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
