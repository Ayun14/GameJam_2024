using UnityEngine;

public class SaveController : MonoSingleton<SaveController>
{
    public void SaveMaxTime(float maxTime)
    {
        PlayerPrefs.SetFloat("MaxTime", maxTime);
    }

    public void SaveHeight(float height)
    {
        PlayerPrefs.SetInt("Height", (int)height);
    }

    public void SaveTime(int min, float sec)
    {
        PlayerPrefs.SetString("Time", string.Format("{0}m {1}s", min, (int)sec));
    }

    public void SaveBGM(float bgm)
    {
        PlayerPrefs.SetFloat("BGM", bgm);
    }

    public void SaveSFX(float sfx)
    {
        PlayerPrefs.SetFloat("SFX", sfx);
    }
}
