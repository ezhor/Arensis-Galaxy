using System;
using Model;
using UnityEngine;

public class PlayerStatusSender : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerStatus _status = GameController.PlayerStatus;
    private PlayerVisuals _playerVisuals;

    private void Awake()
    {
        _playerVisuals = GetComponent<PlayerVisuals>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        _status.Position = transform.position;
        _status.Rotation = transform.rotation;
        _status.Velocity = _rb.velocity;
        _status.FireParticles = _playerVisuals.EmitFireParticles;
    }
}