using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingPlayer : MonoBehaviour
{
    public static float scaleShrinkFactor = 0.008f;
    public static float fallSpeed = 0.008f;

    // Update is called once per frame
    void Start()
    {
        InvokeRepeating("DropToDeath", 0, 4.0f / 600.0f);
    }

    void DropToDeath()
    {
        if (transform.localScale.x <= 0)
            Destroy(gameObject);

        transform.localScale = new Vector3(
            transform.localScale.x - scaleShrinkFactor,
            transform.localScale.y - scaleShrinkFactor,
            transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed, 1);
    }
}
