using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveController : MonoSingleton<SaveController>
{
    [SerializeField] private TextMeshProUGUI _bestHeight;
    [SerializeField] private TextMeshProUGUI _bestTime;

    public void SaveHeight(float height)
    {
        PlayerPrefs.SetInt("Height", (int)height);
    }

    public void SaveTime(int min, float sec)
    {
        PlayerPrefs.SetString("Time", string.Format("{0:D2}:{1:D2}", min, (int)min));
    }

    public void SaveBGM(float bgm)
    {
        PlayerPrefs.SetFloat("BGM", bgm);
    }

    public void SaveSFX(float sfx)
    {
        PlayerPrefs.SetFloat("SFX", sfx);
    }

    public void LoadData()
    {
        if(PlayerPrefs.HasKey("Height"))
            _bestHeight.text = PlayerPrefs.GetInt("Height").ToString() + " M";
        if (PlayerPrefs.HasKey("Time"))
            _bestTime.text = PlayerPrefs.GetString("Time");
    }
}
