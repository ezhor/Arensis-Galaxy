using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SocketIO3;
using Model;
using UnityEngine;

public class SocketClient : MonoBehaviour
{
    [SerializeField]
    private GameObject remotePlayerPrefab;

    private static readonly float DELAY = 0.05f;

    private readonly Dictionary<string, RemotePlayer> _remotePlayers = new Dictionary<string, RemotePlayer>();
    private SocketManager _manager;

    void Start()
    {
        Debug.Log("Connecting to server...");
        _manager = new SocketManager(new Uri("http://mirandaserver.ddns.net:80"));
        _manager.Socket.On(SocketIOEventTypes.Connect, OnConnected);
        _manager.Socket.On<string>("PlayerInfo", OnPlayerInfo);
        _manager.Socket.On<string>("PlayerStatus", OnPlayerStatus);
        _manager.Socket.On<string>("PlayerDisconnected", OnPlayerDisconnected);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        _manager.Close();
    }

    private void OnConnected()
    {
        PlayerInfo info = GameController.PlayerInfo;
        Debug.Log($"Connected to server with info: {info.Name}, {info.Color}");
        _manager.Socket.Emit("PlayerInfo", JsonUtility.ToJson(info));
        StartCoroutine(StatusCoroutine());
    }

    private void OnPlayerInfo(string data)
    {
        PlayerInfo remotePlayerInfo = JsonUtility.FromJson<PlayerInfo>(data);
        if (remotePlayerInfo.ID != _manager.Socket.Id && !_remotePlayers.TryGetValue(remotePlayerInfo.ID, out _))
        {
            _remotePlayers[remotePlayerInfo.ID] =
                Instantiate(remotePlayerPrefab).GetComponentInChildren<RemotePlayer>();
            _remotePlayers[remotePlayerInfo.ID].SetInfo(remotePlayerInfo);
        }
    }

    private IEnumerator StatusCoroutine()
    {
        while (true)
        {
            _manager.Socket.Emit("PlayerStatus", JsonUtility.ToJson(GameController.PlayerStatus));
            yield return new WaitForSecondsRealtime(DELAY);
        }
    }

    private void OnPlayerStatus(string data)
    {
        PlayerStatus playerStatus = JsonUtility.FromJson<PlayerStatus>(data);
        if (playerStatus.ID != _manager.Socket.Id)
        {
            if (_remotePlayers.TryGetValue(playerStatus.ID, out RemotePlayer remotePlayer))
            {
                remotePlayer.UpdateStatus(playerStatus);
            }
            else
            {
                _manager.Socket.Emit("PlayerInfoRequest", playerStatus.ID);
            }
        }
    }
    
    private void OnPlayerDisconnected(string playerId)
    {
        _remotePlayers[playerId].DestroyPlayer();
        _remotePlayers.Remove(playerId);
    }
    
}