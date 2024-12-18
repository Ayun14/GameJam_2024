using Hellmade.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoSingleton<SoundController>
{
    [SerializeField] private List<AudioClip> _bgmClips;
    [SerializeField] private List<AudioClip> _sfxClips;

    public void PlayBGM(int bgmNum)
    {
        EazySoundManager.PlayMusic(_bgmClips[bgmNum]);
    }

    public void PlaySFX(int sfxNum)
    {
        EazySoundManager.PlaySound(_sfxClips[sfxNum]);
    }
}
