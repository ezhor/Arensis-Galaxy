using Sirenix.OdinInspector;
using UnityEngine;

public class Starfield : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private int pixelSize;
    [SerializeField]
    private int pixelsPerUnit;
    [SerializeField]
    private float starDensity;
    [SerializeField]
    private FilterMode filterMode;
#pragma warning restore 0649

    [Button("Generate")]
    private void Awake()
    {
        Texture2D texture = CreateStarfieldTexture();
        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, pixelSize, pixelSize),
            new Vector2(0.5f, 0.5f), pixelsPerUnit);
        
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private Texture2D CreateStarfieldTexture()
    {
        Texture2D texture = new Texture2D(pixelSize, pixelSize);
        for (int x = 0; x < pixelSize; x++)
        {
            for (int y = 0; y < pixelSize; y++)
            {
                texture.SetPixel(x, y, Random.value < starDensity ? Color.white : Color.black);
            }
        }
        texture.filterMode = filterMode;
        
        texture.Apply();
        return texture;
    }
}
