using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.unscaledDeltaTime);

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
