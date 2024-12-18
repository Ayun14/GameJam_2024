using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/AudioClipSelector_Random")]
public class AudioClipSelectorRandom : BaseAudioClipSelector
{
    public override AudioClip SelectAudioClip(IReadOnlyList<AudioClip> clips)
    {
        int cnt = clips.Count;
        int randomIdx = Random.Range(0, cnt);
        return clips[randomIdx];
    }
}