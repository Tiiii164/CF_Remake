//using Unity.Netcode;
//using UnityEngine;

//public class GameManager : NetworkBehaviour
//{
//    [SerializeField] private GameObject playerPrefab; // Prefab của người chơi đã thiết lập NetworkObject

//    public override void OnNetworkSpawn()
//    {
//        if (IsServer) // Kiểm tra nếu đây là server
//        {
//            NetworkManager.OnClientConnectedCallback += OnClientConnected;
//        }
//    }

//    private void OnClientConnected(ulong clientId)
//    {
//        if (!NetworkManager.Singleton.IsClient || !NetworkManager.Singleton.IsConnectedClient)
//        {
//            Debug.LogWarning("Client is not connected or not in client mode.");
//            return;
//        }

//        GameObject playerObject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
//        playerObject.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
//    }

//    private void OnDestroy()
//    {
//        if (IsServer)
//        {
//            NetworkManager.OnClientConnectedCallback -= OnClientConnected;
//        }
//    }
//}
