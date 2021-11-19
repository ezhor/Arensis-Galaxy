using System.Collections.Generic;
using Connection;
using Manager;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayersList : MonoBehaviour
    {
        private SocketClient _socketClient;
        private TextMeshProUGUI _listText;

        private void Awake()
        {
            _socketClient = FindObjectOfType<SocketClient>();
            _listText = GetComponent<TextMeshProUGUI>();
        }

        private void LateUpdate()
        {
            _listText.text = FormatPlayer(GameController.PlayerInfo.Name,
                GameController.PlayerStatus.Position.x, GameController.PlayerStatus.Position.y);

            foreach (KeyValuePair<string, RemotePlayer> keyValuePair in _socketClient.RemotePlayers)
            {
                Vector2 position = keyValuePair.Value.transform.position;
                _listText.text += FormatPlayer(keyValuePair.Value.name, position.x, position.y);
            }
        }

        private string FormatPlayer(string playerName, float x, float y)
        {
            return $"\n{playerName} ({x.ToString("00")},{y.ToString("00")})";
        }
    }
}