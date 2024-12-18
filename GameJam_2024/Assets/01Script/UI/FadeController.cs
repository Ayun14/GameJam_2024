using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _canvasGroup.alpha = 0;
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(true));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        if (isFadeIn)
        {
            _canvasGroup.alpha = 1;
            Tween tween = _canvasGroup.DOFade(0f, 1f);
            yield return tween.WaitForCompletion();
        }
        else
        {
            _canvasGroup.alpha = 0;
            Tween tween = _canvasGroup.DOFade(1f, 1f);
            yield return tween.WaitForCompletion();
        }
    }
}
