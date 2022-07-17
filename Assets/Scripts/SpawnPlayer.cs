using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public Player player;
    public float scaleShrinkFactor = 0.01f;
    public float fallSpeed = 0.01f;
    public int animationFrames = 180;
    private int currentFrame = 0;
    private SerializedPlayerData playerData;

    public Player Init(SerializedPlayerData playerData)
    {
        player.enabled = false;
        this.playerData = playerData;
        currentFrame = 0;
        Vector2 position = TileUtils.GetPosition(playerData.tile);
        transform.position = new Vector3(position.x, position.y + (animationFrames * fallSpeed), -1);
        transform.localScale = new Vector3(
            transform.localScale.x + (animationFrames * scaleShrinkFactor),
            transform.localScale.y + (animationFrames * scaleShrinkFactor),
            1);

        if (playerData.tile.PointsUp)
            transform.localEulerAngles = new Vector3(0, 0, 180);

        player.playerAnimator.GetSprites();
        player.playerAnimator.SetSprite(playerData.side, playerData.rotation);

        return player;
    }

    // Update is called once per frame
    void Update()
    {
        if (++currentFrame == animationFrames)
        {
            transform.localScale = new Vector3(1, 1, 1);
            player.enabled = true;
            player.Init(playerData);
            Destroy(this);
        }

        transform.localScale = new Vector3(
            transform.localScale.x - scaleShrinkFactor,
            transform.localScale.y - scaleShrinkFactor,
            transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed, -1);
    }
}
