using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Testing : MonoBehaviour
{
    private AudioSource _audioSource;
    // Start is called before the first frame update
    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _audioSource.Play();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _audioSource.Stop();
        }
    }
}
