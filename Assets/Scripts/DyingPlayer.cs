using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingPlayer : MonoBehaviour
{
    public float scaleShrinkFactor = 0.01f;
    public float fallSpeed = 0.01f;

    // Update is called once per frame
    void Update()
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
