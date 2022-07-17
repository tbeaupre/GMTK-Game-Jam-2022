using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public AudioSource[] MusicTrackSources;
    public DataManager dataManager;
    public List<AudioSource> Clunks;
    public List<AudioSource> Clinks;
    public List<AudioSource> FloorBoards;

    private Dictionary<SFX_TYPE, List<AudioSource>> SFXLookup;

    public void Start()
    {
        SFXLookup = new Dictionary<SFX_TYPE, List<AudioSource>>
        {
            { SFX_TYPE.CLUNK, Clunks},
            { SFX_TYPE.CLINK, Clinks},
            { SFX_TYPE.FLOORBOARD, FloorBoards},
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
        var pool = SFXLookup[type];
        var source = pool.ElementAt(Random.Range(0, pool.Count()));
        source.Play();
    }
}

public enum SFX_TYPE
{
    CLUNK,
    CLINK,
    FLOORBOARD
}