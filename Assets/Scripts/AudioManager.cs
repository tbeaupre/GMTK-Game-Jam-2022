using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public AudioSource[] MusicTrackSources;
    public DataManager dataManager;
    private List<AudioSource> SFXSources;
    private Dictionary<SFX_TYPE, AudioClip[]> SFXClips;

    public const string SFX_CLUNK = "clunk";
    public const string SFX_CLINK = "clink";
    public const string SFX_FLOORBOARD = "floorboard";

    public void Start()
    {
        SFXSources = new List<AudioSource> {
            gameObject.AddComponent(typeof(AudioSource)) as AudioSource,
            gameObject.AddComponent(typeof(AudioSource)) as AudioSource,
            gameObject.AddComponent(typeof(AudioSource)) as AudioSource
        };

        SFXSources.ForEach(sfx => sfx.volume = 0.6f); // we should tone these down a bit

        SFXClips = new Dictionary<SFX_TYPE, AudioClip[]> {
            {SFX_TYPE.CLUNK,  GetClips(SFX_CLUNK)},
            {SFX_TYPE.CLINK,  GetClips(SFX_CLINK)},
            {SFX_TYPE.FLOORBOARD,  GetClips(SFX_FLOORBOARD)},
        };
    }

    public void Update()
    {
        if (dataManager.levelValue >= 4)
        {
            FadeIn(MusicTrackSources[1]); //lead
        }
        if (dataManager.levelValue >= 5)
        {
            FadeIn(MusicTrackSources[3], 0.7f); // drums
        }
        if (dataManager.levelValue >= 7)
        {
            FadeIn(MusicTrackSources[3]); // drums
            FadeIn(MusicTrackSources[2], 0.7f);  // guitar
        }
    }

    private void FadeIn(AudioSource source, float target = 1.0f)
    {
        var increment = 0.002f;
        if (source.volume < target)
        {
            source.volume += increment;
        }
    }

    public void PlaySFX(SFX_TYPE type)
    {
        var source = SFXSources.FirstOrDefault(s => !s.isPlaying);
        if (source == null) return;
        var pool = SFXClips[type];
        source.clip = pool[Random.Range(0, pool.Count())];
        source.Play();
    }

    private AudioClip[] GetClips(string prefix)
    {
        var path = Application.dataPath + "/Resources/SFX";
        Debug.Log(path);
        string[] sfx_files = Directory.GetFiles(path).Where(f => f.Contains(prefix) && f.EndsWith(".wav")).ToArray();
        // sfx_files.ToList().ForEach(f => Debug.Log(f));
        return sfx_files
            .Select(f => {
                string s = "SFX" + f.Replace(path, "").Replace(".wav", "").Replace("\\", "/");
                Debug.Log(s);
                return s;
                })

            /*.Select(f => AssetDatabase.LoadAssetAtPath(f, typeof(AudioClip)) as AudioClip)*/
            .Select(f => Resources.Load<AudioClip>(f))
            .ToArray();
    }
}

public enum SFX_TYPE
{
    CLUNK,
    CLINK,
    FLOORBOARD
}