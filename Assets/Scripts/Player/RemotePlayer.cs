using Model;
using UnityEngine;

namespace Player
{
    public class RemotePlayer : MonoBehaviour
    {
        private Rigidbody2D rb;
        private PlayerVisuals _playerVisuals;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _playerVisuals = GetComponent<PlayerVisuals>();
        }

        public void SetInfo(PlayerInfo playerInfo)
        {
            GetComponent<PlayerVisuals>().SetPlayerVisuals(playerInfo.Name, playerInfo.Color);
        }

        public void UpdateStatus(PlayerStatus playerStatus)
        {
            transform.position = playerStatus.Position;
            transform.rotation = playerStatus.Rotation;
            rb.velocity = playerStatus.Velocity;
            _playerVisuals.EmitFireParticles = playerStatus.FireParticles;
        }

        public void DestroyPlayer()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}