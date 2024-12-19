using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _endingTimeline;
    [SerializeField] private CanvasGroup _fadeCanvas;
    [SerializeField] private CanvasGroup _endingCanvas;
    [SerializeField] private CanvasGroup _keyInputCanvas;
    [SerializeField] private Canvas _meterCanvas;
    [SerializeField] private MeterController _meterController;


    private Player _player;

    private void Awake()
    {
        _endingTimeline = GetComponent<PlayableDirector>();
    }

    public void EndTimeline()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        _fadeCanvas.alpha = 0;
        _endingCanvas.alpha = 0;
        _keyInputCanvas.alpha = 0;
        Tween tween = _fadeCanvas.DOFade(1f, 2.5f);
        yield return tween.WaitForCompletion();
        tween = _endingCanvas.DOFade(1f, 2f);
        yield return tween.WaitForCompletion();
        _keyInputCanvas.DOFade(1f, 1f);

        if (_player != null)
            _player.InputCompo.isAnyKeyPress = true;

        _endingTimeline.Stop();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _meterController.SaveTime();
            _player = player;
            _meterCanvas.enabled = false;
            player.StartEndingTimeline(_endingTimeline);
        }
    }
}
