using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerVisuals : MonoBehaviour
    {
        public bool EmitFireParticles { get; set; }

        private static readonly int NewColor = Shader.PropertyToID("NewColor");

        private ParticleSystem _fireParticles;

        private void Awake()
        {
            _fireParticles = GetComponentInChildren<ParticleSystem>();
        }

        private void Update()
        {
            if (EmitFireParticles)
            {
                if (!_fireParticles.isEmitting)
                {
                    _fireParticles.Play();
                }
            }
            else
            {
                _fireParticles.Stop();
            }
        }

        public void SetPlayerVisuals(string playerName, Color color)
        {
            transform.parent.GetComponentInChildren<TextMeshPro>().text = playerName;
            GetComponentInChildren<SpriteRenderer>().material.SetColor(NewColor, color);
        }
    }
}