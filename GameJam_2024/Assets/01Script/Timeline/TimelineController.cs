using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _endingTimeline;
    [SerializeField] private Player _player;
    [SerializeField] private CanvasGroup _fadeCanvas;

    private void Awake()
    {
        _endingTimeline = GetComponent<PlayableDirector>();
    }

    public void SetPlayerPosition()
    {
        _player.GetComponent<Animator>().enabled = true;
    }

    public void EndTimeline()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        _fadeCanvas.alpha = 0;
        Tween tween = _fadeCanvas.DOFade(1f, 1f);
        yield return tween.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("TitleScene");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _endingTimeline.Play();
        }
    }
}
