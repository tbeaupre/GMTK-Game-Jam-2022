using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private static Sprite[] sprites;
    private const int SPRITESHEET_WIDTH = 8;

    public void GetSprites()
    {
        if (sprites == null)
        {
            Debug.Log("Loading Sprites");
            sprites = Resources.LoadAll<Sprite>("Sprites/octahedron3");
        }
    }

    public void SetSprite(int side, int sideRotation)
    {
        int y = sideRotation switch {
            0 => 0,
            1 => 2,
            2 => 1,
            3 => 0,
            4 => 2,
            _ => 1
        };
        spriteRenderer.sprite = sprites[y * SPRITESHEET_WIDTH + side - 1];
    }
}
