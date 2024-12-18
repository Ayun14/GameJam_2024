using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_DebugBase<T> : OJYSewer.MonoSingleton<T> where T : UI_DebugBase<T>
{
    private const bool DISABLE_ALL_UI = false;
    [SerializeField] private bool active = true;
    public bool ShowDebugUI
    {
        get => active && !DISABLE_ALL_UI;
        set
        {
            active = value;
            OnChange();
        }
    }
    [SerializeField] private List<Text> list;
    public IList<Text> GetList => list;
    protected virtual void Start()
    {
        OnChange();
    }
    private void OnChange()
    {
        gameObject.SetActive(ShowDebugUI);
    }

}
