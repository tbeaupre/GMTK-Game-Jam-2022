using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private Sprite[] sprites;
    private const int SPRITESHEET_WIDTH = 8;

    public void GetSprites()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/octahedron3");
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