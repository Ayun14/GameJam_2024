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

    public void ArrowSetActive(bool active)
    {

    }
}
