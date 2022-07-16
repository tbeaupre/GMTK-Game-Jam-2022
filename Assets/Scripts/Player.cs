using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TileData tile;

    public int side = 1;
    public int sideRotation = 1;
    public AudioManager AudioMgmt;
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        AudioMgmt = GetComponent<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var PointsDown = tile.PointsUp; // its going to be the opposite of whatever tile it's on
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            side = RotateInDirection(side, sideRotation, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && PointsDown)
        {
            side = RotateInDirection(side, sideRotation, 1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !PointsDown)
        {
            side = RotateInDirection(side, sideRotation, 2);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            side = RotateInDirection(side, sideRotation, 3);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !PointsDown)
        {
            side = RotateInDirection(side, sideRotation, 4);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && PointsDown)
        {
            side = RotateInDirection(side, sideRotation, 5);
        }

        sideRotation = (sideRotation + 6) % 6;
        ChangeFrame(side, sideRotation);
        transform.localEulerAngles = new Vector3(0, 0, PointsDown ? 180 : 0);
        UpdatePosition();
    }

    int RotateInDirection(int side, int rotation, int direction)
    {
        // Debug.Log($"{side} {rotation} {direction}");

        // Don't do anything if face is pressed
        if ((rotation + direction) % 2 == 1)
            return side;

        AudioMgmt.PlaySFX(SFX_TYPE.CLUNK);

        switch (direction)
        {
            case 0:
                ++tile.B;
                break;
            case 1:
                --tile.C;
                break;
            case 2:
                ++tile.A;
                break;
            case 3:
                --tile.B;
                break;
            case 4:
                ++tile.C;
                break;
            case 5:
                --tile.A;
                break;
        }

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

    public int GetOppositeSide() => side switch {
        1 => 6,
        2 => 5,
        3 => 8,
        4 => 7,
        5 => 2,
        6 => 1,
        7 => 4,
        _ => 3
    };

    public void ChangeFrame(int x, int y)
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
        Vector2 pos2d = TileUtils.GetPosition(tile);
        transform.position = new Vector3(pos2d.x, pos2d.y, -1);
    }

    public void Init(int side, int rotation, TileData tile)
    {
        this.side = side;
        this.sideRotation = rotation;
        this.tile = new TileData(tile);
    }

    public void Init(SerializedPlayerData playerData)
    {
        this.side = playerData.side;
        this.sideRotation = playerData.rotation;
        this.tile = new TileData(playerData.tile);
    }
}
