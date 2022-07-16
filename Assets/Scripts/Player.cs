using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int a = 4;
    public int b = -7;
    public int c = 4;

    public int side;
    public int sideRotation;
    public bool isUpsideDown;
    public AudioManager AudioMgmt;
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            side = RotateInDirection(side, sideRotation, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            side = RotateInDirection(side, sideRotation, 1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            side = RotateInDirection(side, sideRotation, 2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            side = RotateInDirection(side, sideRotation, 3);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            side = RotateInDirection(side, sideRotation, 4);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            side = RotateInDirection(side, sideRotation, 5);

        sideRotation = (sideRotation + 6) % 6;
        ChangeFrame(side, sideRotation);
        isUpsideDown = sideRotation % 2 == 1;
        transform.localEulerAngles = new Vector3(0, 0, isUpsideDown ? 180 : 0);
    }

    int RotateInDirection(int side, int rotation, int direction)
    {
        // Debug.Log($"{side} {rotation} {direction}");

        // Don't do anything if face is pressed
        if ((rotation + direction) % 2 == 1)
            return side;

        AudioMgmt.PlaySFX();

        if (((direction - rotation + 6) % 6) == 0)
        {
            sideRotation += 3;
            return side switch {
                1 => 2,
                2 => 1,
                3 => 4,
                4 => 3,
                5 => 6,
                6 => 5,
                7 => 8,
                _ => 7
            };
        }
        if (((direction - rotation + 6) % 6) == 2)
        {
            sideRotation += 1;
            return side switch {
                1 => 4,
                2 => 7,
                3 => 2,
                4 => 5,
                5 => 8,
                6 => 3,
                7 => 6,
                _ => 1
            };
        }
        sideRotation -= 1;
        return side switch {
            1 => 8,
            2 => 3,
            3 => 6,
            4 => 1,
            5 => 4,
            6 => 7,
            7 => 2,
            _ => 5
        };
    }

    void ChangeFrame(int x, int y)
    {
        int correctedY = y switch {
            0 => 0,
            1 => 2,
            2 => 1,
            3 => 0,
            4 => 2,
            _ => 1
        };
        spriteRenderer.sprite = sprites[correctedY * 8 + x - 1];
    }

    void UpdatePosition()
    {
        Vector2 pos2d = TileUtils.GetPosition(new TileData(a, b, c));
        transform.position = new Vector3(pos2d.x, pos2d.y, -1);
    }
}
