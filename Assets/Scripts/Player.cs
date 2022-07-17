using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TileData tile;

    public int side = 1;
    public int sideRotation = 1;
    public AudioManager AudioMgmt;
    public PlayerAnimator playerAnimator;

    // Update is called once per frame
    public void Update()
    {
        var direction = getDirectionFromInput();
        var nextSide = direction >= 0 ? RotateInDirection(direction) : null;
        if (nextSide != null)
        {
            AudioMgmt.PlaySFX(SFX_TYPE.CLUNK);
            side = nextSide ?? side;
            sideRotation = (sideRotation + 6) % 6;
            playerAnimator.SetSprite(side, sideRotation);
            transform.localEulerAngles = new Vector3(0, 0, PointsDown ? 180 : 0);
            UpdatePosition();
        }
    }

    private int getDirectionFromInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) return 0;
        if (Input.GetKeyDown(KeyCode.RightArrow) && PointsDown) return 1;
        if (Input.GetKeyDown(KeyCode.RightArrow) && !PointsDown) return 2;
        if (Input.GetKeyDown(KeyCode.DownArrow)) return 3;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !PointsDown) return 4;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && PointsDown) return 5;
        return -1;
    }

    private bool PointsDown => tile.PointsUp; // its going to be the opposite of whatever tile it's on

    int? RotateInDirection(int direction)
    {
        // Debug.Log($"{side} {rotation} {direction}");

        // Don't do anything if face is pressed
        if ((sideRotation + direction) % 2 == 1)
            return null;

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

        if (((direction - sideRotation + 6) % 6) == 0)
        {
            this.sideRotation += 3;
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
        if (((direction - sideRotation + 6) % 6) == 2)
        {
            this.sideRotation += 1;
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
        this.sideRotation -= 1;
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
