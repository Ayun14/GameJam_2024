using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/AudioClipSelector_Default")]
public class AudioClipSelectorDefault : BaseAudioClipSelector
{
    public override AudioClip SelectAudioClip(IReadOnlyList<AudioClip> clips)
    {
        return clips[0];
    }
}
