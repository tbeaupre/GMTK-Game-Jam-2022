using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterText : MonoBehaviour
{
    public string Base;
    public int Value;
    public TextMeshProUGUI Mesh;

    // Update is called once per frame
    void Update() => Mesh.text = $"{Base}: {ValueString}";
    string ValueString => Value != -1 ? Value.ToString() : "?";
}
