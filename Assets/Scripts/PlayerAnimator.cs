using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private static Sprite[] sideSprites;
    private static Sprite[] edgeSprites;
    private const int SPRITESHEET_WIDTH = 8;

    private int prevSide;
    private int side;
    private int additionalRotation;


    public void GetSprites()
    {
        if (sideSprites == null)
        {
            Debug.Log("Loading Side Sprites");
            sideSprites = Resources.LoadAll<Sprite>("Sprites/octahedron3");
        }
        if (edgeSprites == null)
        {
            Debug.Log("Loading Edge Sprites");
            edgeSprites = Resources.LoadAll<Sprite>("Sprites/octahedron-edges");
        }
    }

    public void RollInDirection(int side, int sideRotation, int direction)
    {
        StopAllCoroutines();
        ResetRotation(); // Reset rotation from potentially interrupted previous animation

        this.prevSide = this.side;
        this.side = side;
        IEnumerator coroutine = RollInDirectionCo(side, sideRotation, direction);
        StartCoroutine(coroutine);
    }

    IEnumerator RollInDirectionCo(int side, int sideRotation, int direction)
    {
        // First Frame;

        if (direction == 1 || direction == 4)
            additionalRotation = 120;
        if (direction == 2 || direction == 5)
            additionalRotation = 240;

        transform.Rotate(new Vector3(0, 0, 180 + additionalRotation));
        SetEdgeSprite(prevSide, side);

        yield return new WaitForSeconds(0.1f);

        // Second Frame;
        ResetRotation();
        SetSprite(side, sideRotation);
    }

    private void ResetRotation()
    {
        transform.Rotate(new Vector3(0, 0, -additionalRotation));
        additionalRotation = 0;
    }

    private void SetEdgeSprite(int prevSide, int newSide)
    {
        spriteRenderer.sprite = edgeSprites[((prevSide - 1) * SPRITESHEET_WIDTH) + newSide - 1];
    }

    public void SetSprite(int side, int sideRotation)
    {
        this.side = side;

        int y = sideRotation switch {
            0 => 0,
            1 => 2,
            2 => 1,
            3 => 0,
            4 => 2,
            _ => 1
        };

        spriteRenderer.sprite = sideSprites[y * SPRITESHEET_WIDTH + side - 1];
    }
}
