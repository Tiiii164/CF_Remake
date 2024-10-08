using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using static LobbyManager;

public class GameLobby : MonoBehaviour
{
    private Lobby hostLobby;
    private Lobby joinedLobby;

    private float heartbeatTimer;
    private float lobbyUpdateTimer;

    private string playerName;
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

            
        playerName = "Thanh Binh" + UnityEngine.Random.Range(10, 101);
        Debug.Log("Hello " + playerName);
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    //hàm này cứ 15s nó chạy 1 lần, giữ cho lobby còn tồn tại
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
    
    //hàm này là cứ 1,1s là nó update cái lobby data v thui
    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

                joinedLobby = lobby;
            }
        }
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby" + UnityEngine.Random.Range(10,100);
            int maxPlayer = 4;

            //cái CreateLobbyOptions là để mình thiết lập mấy cái data khác của lobby như Private,  password gì đó.
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    // loại data có thể phân loại và mình có thể dùng data này bỏ dô cái search của mình để
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "Crossroads of Death") },
                }
                
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer , createLobbyOptions);

            hostLobby = lobby;
            joinedLobby = hostLobby;

            Debug.Log("Created Lobby: " + lobby.Name + " " + lobby.MaxPlayers + "  ID: " + lobby.Id + " Code: "+ lobby.LobbyCode);

            PrintPlayers(hostLobby);
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
                Debug.Log(lobby.Name + ". Maxplayer: " + lobby.MaxPlayers+" Game Mode: " + lobby.Data["GameMode"].Value);
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
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);

            joinedLobby = lobby;

            Debug.Log("Joined lobby with code: " + lobbyCode);

            PrintPlayers(lobby);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
    }
    public async void QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions quickJoinLobbyOptions = new QuickJoinLobbyOptions
            {
                Player = GetPlayer(),
            };

            Lobby lobby =  await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);

            joinedLobby = lobby;

            Debug.Log("Quick Join successfully, Lobby name: "+ joinedLobby.Name);

            PrintPlayers(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
    }

    private Player GetPlayer()
    {
        return new Player
        {
            //dô trong cái player Data xong set cái name thành name mình đặt oke chưa 
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "PlayerName",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) },
            }
        };
    }


    public void PrintPlayers()
    {
        PrintPlayers(joinedLobby);
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in Lobby: "+ lobby.Name + " GameMode: "+ lobby.Data["GameMode"].Value);
        foreach (Player player in lobby.Players)
        {
            Debug.Log("Player ID: "+ player.Id + " PlayerName: "+ player.Data["PlayerName"].Value);
        }
    }

    public async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            //sài cái làm UpdateLobbyAsync để update data của các lobbies
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
            {
                { "GameMode" , new DataObject(DataObject.VisibilityOptions.Public,gameMode) }
            }
            });

            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
        
    }

    public async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId,
                new UpdatePlayerOptions
                {
                    Data = new Dictionary<string, PlayerDataObject>
                    {
                        { "PlayerName",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, newPlayerName) },
                    }
                }
                );

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
           await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id,AuthenticationService.Instance.PlayerId);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
    }

    public async void KickPlayer()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
    }

    //chuyển host ID sang thằng khác
    public async void MygrateLobbyHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedLobby.Players[1].Id,
            });

            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
    }

    public async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
    }
}
