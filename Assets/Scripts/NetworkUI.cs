using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TextMeshProUGUI playerCountText;

    private NetworkVariable<int> playersNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    private void Update()
    {
        playerCountText.text = "Players: " + playersNum.Value.ToString();

        if (!IsServer) return;
        playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
}


//using System.Collections.Generic;
//using TMPro;
//using Unity.Netcode;
//using UnityEngine;
//using UnityEngine.UI;

//public class NetworkUI : NetworkBehaviour
//{
//    [SerializeField] private Button hostButton;
//    [SerializeField] private Button clientButton;
//    [SerializeField] private TextMeshProUGUI playerCountText;

//    private NetworkVariable<int> playersNum = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone);
//    private void Awake()
//    {
//        hostButton.onClick.AddListener(() =>
//        {
//            NetworkManager.Singleton.StartHost();
//            AssignOwnership(NetworkManager.Singleton.LocalClientId); // Gán quyền sở hữu cho host
//        });

//        clientButton.onClick.AddListener(() =>
//        {
//            NetworkManager.Singleton.StartClient();
//            NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
//            {
//                AssignOwnership(clientId); // Gán quyền sở hữu cho client
//            };
//        });
//    }

//    private void AssignOwnership(ulong clientId)
//    {
//        var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
//        playerObject.GetComponent<NetworkObject>().ChangeOwnership(clientId);
//    }

//    private void Update()
//    {
//        playerCountText.text = "Players: " + playersNum.Value.ToString();

//        if (!IsServer) return;
//        playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
//    }
//}
