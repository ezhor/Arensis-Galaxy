using System.Collections;
using System.Collections.Generic;
using Model;
using Socket.Quobject.SocketIoClientDotNet.Client;
using UnityEngine;

public class SocketClient : MonoBehaviour
{
    [SerializeField]
    private GameObject remotePlayerPrefab;
    
    private static readonly float DELAY = 0.05f;

    private QSocket _socket;
    private readonly Dictionary<string, RemotePlayer> _remotePlayers = new Dictionary<string, RemotePlayer>();

    void Start()
    {
        Debug.Log("Connecting to server...");
        _socket = IO.Socket("http://localhost:3000");

        _socket.On(QSocket.EVENT_CONNECT, () =>
        {
            Debug.Log($"Connected to server with info: {GameController.PlayerInfo}");
            _socket.Emit("PlayerInfo", GameController.PlayerInfo);
            StartCoroutine(StatusCoroutine());
        });

        _socket.On("PlayerInfo", remotePlayerInfo =>
        {
            HandleRemotePlayerInfo((PlayerInfo)remotePlayerInfo);
        });
    }
    
    private void OnDestroy()
    {
        _socket.Disconnect();
    }

    private IEnumerator StatusCoroutine()
    {
        _socket.Emit("PlayerStatus", GameController.PlayerStatus);
        yield return new WaitForSecondsRealtime(DELAY);
    }

    private void HandleRemotePlayerInfo(PlayerInfo remotePlayerInfo)
    {
        if (!_remotePlayers.TryGetValue(remotePlayerInfo.ID, out _))
        {
            _remotePlayers[remotePlayerInfo.ID] = Instantiate(remotePlayerPrefab).GetComponent<RemotePlayer>();
            _remotePlayers[remotePlayerInfo.ID].SetInfo(remotePlayerInfo);
        }
    }
}