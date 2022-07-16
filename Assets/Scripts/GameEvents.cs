using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameEvents : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider)
            {
                Debug.Log("ray");
                if (hit.transform != null)
                {
                    GameObject target = hit.transform.gameObject;
                    if (target.TryGetComponent<Tile>(out var tile))
                    {
                        var tileData = tile.Data;
                        Debug.Log($"{tileData.A}, {tileData.B}, {tileData.C}");
                    }
                }
            }
        }
    }
}
