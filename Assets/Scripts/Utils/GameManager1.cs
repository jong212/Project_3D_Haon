using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{


    void Start()
    {

        NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
        if (RelayManager.Instance.IsHost)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionApproval;

            var hostConnectionInfo = RelayManager.Instance.GetHostConnectionInfo();
            LogConnectionInfo(hostConnectionInfo.AllocationId, hostConnectionInfo.Key, hostConnectionInfo.ConnectionData, null);

            if (hostConnectionInfo.AllocationId == null || hostConnectionInfo.Key == null || hostConnectionInfo.ConnectionData == null)
            {
                Debug.LogError("Host connection info contains null values.");
                return;
            }

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                hostConnectionInfo.dtlsAddress,
                (ushort)hostConnectionInfo.dtlsPort,
                hostConnectionInfo.AllocationId,
                hostConnectionInfo.Key,
                hostConnectionInfo.ConnectionData,
                true
            );
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            var clientConnectionInfo = RelayManager.Instance.GetClientConnectionInfo();
            LogConnectionInfo(clientConnectionInfo.AllocationId, clientConnectionInfo.Key, clientConnectionInfo.ConnectionData, clientConnectionInfo.HostConnectionData);

            if (clientConnectionInfo.AllocationId == null || clientConnectionInfo.Key == null || clientConnectionInfo.ConnectionData == null || clientConnectionInfo.HostConnectionData == null)
            {
                Debug.LogError("Client connection info contains null values.");
                return;
            }

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                clientConnectionInfo.dtlsAddress,
                (ushort)clientConnectionInfo.dtlsPort,
                clientConnectionInfo.AllocationId,
                clientConnectionInfo.Key,
                clientConnectionInfo.ConnectionData,
                clientConnectionInfo.HostConnectionData,
                true
            );
            NetworkManager.Singleton.StartClient();
        }
    }

    private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true;
        response.Pending = false;
    }

    private void LogConnectionInfo(byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData)
    {
        Debug.Log($"AllocationId: {ByteArrayToString(allocationId)}");
        Debug.Log($"Key: {ByteArrayToString(key)}");
        Debug.Log($"ConnectionData: {ByteArrayToString(connectionData)}");
        Debug.Log($"HostConnectionData: {ByteArrayToString(hostConnectionData)}");
    }
    private string ByteArrayToString(byte[] byteArray)
    {
        return byteArray == null ? "null" : BitConverter.ToString(byteArray);
    }
}
