using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
    private Lobby hostLobby;
    private float heartbeatTimer;
    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();



        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed In" + AuthenticationService.Instance.PlayerId);
        };
        //có thể update lên thành sign in with gg hay fb gì đó
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    private async void HandleLobbyHeartbeat()
    {
        if(hostLobby != null)
        {
            heartbeatTimer -=Time.deltaTime;
            if(heartbeatTimer < 0)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
    
    public async void CreatLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayer = 4;

            //cái CreateLobbyOptions là để mình thiết lập mấy cái khác của lobby như Private,  password gì đó.
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = true,
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer , createLobbyOptions);

            hostLobby = lobby;

            Debug.Log("Created Lobby: " + lobby.Name + " " + lobby.MaxPlayers + "  ID: " + lobby.Id + " Code: "+ lobby.LobbyCode);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
        
    }
    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25, // search ra 25 cái lobbies>
                Filters = new List<QueryFilter> //filer này là lọc ra lobby nào có AvailableSlots GreaterThan (GT) > 0 
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false ,QueryOrder.FieldOptions.Created) // sort từ cũ đến mới 
                }
            };
            
            //cái parameter trongQueryLobbiesAsync() là filter, nếu rỗng là show toàn bộ
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            Debug.Log("Lobbies Found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + ". Maxplayer: " + lobby.MaxPlayers);
            }
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
        
       
    }

    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            
            await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
            Debug.Log("Joined lobby with code: " + lobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
    }
 
}
