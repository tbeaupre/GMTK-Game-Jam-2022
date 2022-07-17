using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            gameManager.Play();
            Destroy(gameObject);
        }
    }
}
