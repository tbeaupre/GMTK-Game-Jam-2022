using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public AudioSource SFXSource;
    private Dictionary<SFX_TYPE, AudioClip[]> SFXClips;

    public const string SFX_CLUNK = "clunk";
    public const string SFX_CLINK = "clink";
    public const string SFX_FLOORBOARD = "floorboard";

    public void Start()
    {
        SFXSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        SFXClips = new Dictionary<SFX_TYPE, AudioClip[]> {
            {SFX_TYPE.CLUNK,  GetClips(SFX_CLUNK)},
            {SFX_TYPE.CLINK,  GetClips(SFX_CLINK)},
            {SFX_TYPE.FLOORBOARD,  GetClips(SFX_FLOORBOARD)},
        };
    }

    public void PlaySFX(SFX_TYPE type)
    {
        var pool = SFXClips[type];
        SFXSource.clip = pool[Random.Range(0, pool.Count())];
        SFXSource.Play();
    }

    private AudioClip[] GetClips(string prefix)
    {
        var path = Application.dataPath + "/SFX";
        string[] sfx_files = Directory.GetFiles(path).Where(f => f.Contains(prefix) && f.EndsWith(".wav")).ToArray();
        return sfx_files
            .Select(f => "Assets/SFX/" + f.Replace(path, ""))
            .Select(f => AssetDatabase.LoadAssetAtPath(f, typeof(AudioClip)) as AudioClip)
            .ToArray();
    }
}

public enum SFX_TYPE
{
    CLUNK,
    CLINK,
    FLOORBOARD
}