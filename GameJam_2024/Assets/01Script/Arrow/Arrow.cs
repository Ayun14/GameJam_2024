using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector3 direction = _player.GetMouseDirection(transform);
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.unscaledDeltaTime);

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void ArrowSetActive(bool active, float time)
    {
        if (active)
            gameObject.SetActive(active);

        StartCoroutine(ScaleObject(active ? Vector3.one : Vector3.zero, time));

        if (!active)
            gameObject.SetActive(active);
    }

    private IEnumerator ScaleObject(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = targetScale == Vector3.zero ? Vector3.one : Vector3.zero;
        transform.localScale = initialScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
