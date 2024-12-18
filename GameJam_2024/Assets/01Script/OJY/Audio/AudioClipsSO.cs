using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/AudioClipSO")]
public class AudioClipsSO : ScriptableObject
{
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private BaseAudioClipSelector selector;
    public IReadOnlyList<AudioClip> GetClips => audioClips;
    public AudioClip SelectedAudioClip => selector.SelectAudioClip(GetClips);
    [Obsolete("use Other Play Method instead, this creates object")]
    public void Play(AudioClip clip, Vector3 position, float volume = 1) => AudioSource.PlayClipAtPoint(clip, position, volume);
    public void Play(AudioClip clip, AudioSource audioSource, float volume = 1) => audioSource.PlayOneShot(clip, volume); 
}
