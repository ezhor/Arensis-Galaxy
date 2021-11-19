using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SocketIO3;
using Manager;
using Model;
using Player;
using UnityEngine;

namespace Connection
{
    public class SocketClient : MonoBehaviour
    {
        [SerializeField] private GameObject remotePlayerPrefab;

        private static readonly float DELAY = 0.05f;

        public Dictionary<string, RemotePlayer> RemotePlayers { get; } = new Dictionary<string, RemotePlayer>();

        private const string SERVER_URL = "https://6106c6d3-8032-430f-bdbf-bf51c216cd80.clouding.host:81";
        private SocketManager _manager;

        void Start()
        {
            Debug.Log($"Connecting to {SERVER_URL} ...");
            _manager = new SocketManager(new Uri(SERVER_URL));
            _manager.Socket.On(SocketIOEventTypes.Connect, OnConnected);
            _manager.Socket.OnTypedData<PlayerInfo>("PlayerInfo", OnPlayerInfo);
            _manager.Socket.OnTypedData<PlayerStatus>("PlayerStatus", OnPlayerStatus);
            _manager.Socket.On<string>("PlayerDisconnected", OnPlayerDisconnected);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            _manager.Close();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            Focus(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Focus(!pauseStatus);
        }

        private void Focus(bool focus)
        {
            if (!focus)
            {
                GameController.PlayerStatus.Velocity = Vector2.zero;
                GameController.PlayerStatus.FireParticles = false;
                SendPLayerStatus();
            }
        }

        private void OnConnected()
        {
            PlayerInfo info = GameController.PlayerInfo;
            Debug.Log($"Connected to server with info: {info.Name}, {info.Color}");
            _manager.Socket.Emit("PlayerInfo", JsonUtility.ToJson(info));
            StartCoroutine(StatusCoroutine());
        }

        private void OnPlayerInfo(PlayerInfo remotePlayerInfo)
        {
            if (remotePlayerInfo.ID != _manager.Socket.Id && !RemotePlayers.TryGetValue(remotePlayerInfo.ID, out _))
            {
                RemotePlayers[remotePlayerInfo.ID] =
                    Instantiate(remotePlayerPrefab).GetComponentInChildren<RemotePlayer>();
                RemotePlayers[remotePlayerInfo.ID].SetInfo(remotePlayerInfo);
                RemotePlayers[remotePlayerInfo.ID].name = remotePlayerInfo.Name;
            }
        }

        private IEnumerator StatusCoroutine()
        {
            while (true)
            {
                SendPLayerStatus();
                yield return new WaitForSecondsRealtime(DELAY);
            }
        }

        private void SendPLayerStatus()
        {
            _manager.Socket.Emit("PlayerStatus", JsonUtility.ToJson(GameController.PlayerStatus));
        }

        private void OnPlayerStatus(PlayerStatus playerStatus)
        {
            if (playerStatus.ID != _manager.Socket.Id)
            {
                if (RemotePlayers.TryGetValue(playerStatus.ID, out RemotePlayer remotePlayer))
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
            RemotePlayers[playerId].DestroyPlayer();
            RemotePlayers.Remove(playerId);
        }
    }
}