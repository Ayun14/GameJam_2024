using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAudioClipSelector : ScriptableObject
{
    public abstract AudioClip SelectAudioClip(IReadOnlyList<AudioClip> clips);
}
