using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource SFXSource;
    private AudioClip[] clunks;
    private AudioClip[] clinks;
    private AudioClip[] floorboards;


    public void Start()
    {
        SFXSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;

        clunks = GetClips("clunk");
        clinks = GetClips("clink");
        floorboards = GetClips("floorboard");
    }

    public void PlaySFX()
    {
        SFXSource.clip = clunks[Random.Range(0, clunks.Count())];
        SFXSource.Play();
    }

    private AudioClip[] GetClips(string prefix)
    {
        ArrayList al = new ArrayList();
        var path = Application.dataPath + "/SFX";
        string[] sfx_files = Directory.GetFiles(path).Where(f => f.Contains(prefix) && f.EndsWith(".wav")).ToArray();
        Debug.Log(sfx_files[0]);
        return sfx_files
            .Select(f => "Assets/SFX/" + f.Replace(path, ""))
            .Select(f => AssetDatabase.LoadAssetAtPath(f, typeof(AudioClip)) as AudioClip)
            .ToArray();
    }

}
