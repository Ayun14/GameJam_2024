using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fadeCanvas;
    private void Start()
    {
        SoundController.Instance.PlayBGM(1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        _fadeCanvas.alpha = 0;
        Tween tween = _fadeCanvas.DOFade(1f, 2.5f);
        yield return tween.WaitForCompletion();
        SceneManager.LoadScene("MainScene");
    }
}
