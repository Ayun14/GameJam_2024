using Hellmade.Sound;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoSingleton<SoundController>
{
    [SerializeField] private List<AudioClip> _bgmClips;
    [SerializeField] private List<AudioClip> _sfxClips;

    private void Start()
    {
        if (SceneManager.GetSceneByName("MainScene") == SceneManager.GetActiveScene())
            PlayBGM(1);
    }

    public void PlayBGM(int bgmNum)
    {
        EazySoundManager.PlayMusic(_bgmClips[bgmNum]);
    }

    public void PlaySFX(int sfxNum, float volume = 1)
    {
        EazySoundManager.PlaySound(_sfxClips[sfxNum], volume);
    }
}
