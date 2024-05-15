using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class RelayManager : Singleton<RelayManager>
{
    private string joinCode;
    private string ip;
    private int port;
    private byte[] connectionData;
    private System.Guid allocationId;

    public async Task<string> CreateRelay(int maxConnection)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        RelayServerEndpoint dtlsEnpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
        ip = dtlsEnpoint.Host;
        port = dtlsEnpoint.Port;

        allocationId = allocation.AllocationId;
        connectionData = allocation.ConnectionData;

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
        connectionData = allocation.ConnectionData;

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

}
