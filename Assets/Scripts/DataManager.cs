using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public TriangleGrid Grid;

    // Start is called before the first frame update
    void Start()
    {
        Grid = new TriangleGrid(9);
    }
}
