using Sirenix.OdinInspector;
using UnityEngine;

namespace Environment
{
    public class Boundaries : MonoBehaviour
    {

#pragma warning disable 0649
        [SerializeField]
        private BoxCollider2D top;
        [SerializeField]
        private BoxCollider2D bottom;
        [SerializeField]
        private BoxCollider2D left;
        [SerializeField]
        private BoxCollider2D right;
        [SerializeField]
        private SpriteRenderer starfield;
        [SerializeField]
        private int width;
#pragma warning restore 0649

        private void Start()
        {
            Resize();
        }

        private void OnValidate()
        {
            Resize();
        }

        [Button("Force resize")]
        public void Resize()
        {
            int size = (int)(starfield.sprite.texture.width / starfield.sprite.pixelsPerUnit);

            top.transform.position = new Vector2(0f, size / 2f - width / 2f);
            bottom.transform.position = new Vector2(0f, -size / 2f + width / 2f);
            left.transform.position = new Vector2(-size / 2f + width / 2f, 0f);
            right.transform.position = new Vector2(size / 2f - width / 2f, 0f);

            top.size = new Vector2(size, width);
            bottom.size = new Vector2(size, width);
            right.size = new Vector2(width, size);
            left.size = new Vector2(width, size);
        }
    }
}
