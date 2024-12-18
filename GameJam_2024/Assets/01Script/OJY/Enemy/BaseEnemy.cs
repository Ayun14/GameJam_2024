using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private enum SpawnDirection
    {
        None,
        Right,
        Left
    }
    [Header("Bullet Settings")]
    [SerializeField] private BaseBulletInstantiatorSO instansiaor;
    [SerializeField] private BaseBullet bulletPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private SpawnDirection spawnDirection;

    [Header("Shoot Settings")]
    [SerializeField] private float repeatDuration;
    private float timer = 0;
    //[SerializeField] private bool parabolaPreview;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= repeatDuration)
        {
            timer = 0;
            FireBullet();
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //    FireBullet();
    }
    private void FireBullet()
    {
        Vector3 direction = Vector3.zero;
        switch (spawnDirection)
        {
            case SpawnDirection.Right:
                direction = firePos.right;
                break;
            case SpawnDirection.Left:
                direction = -firePos.right;
                break;
        }
        instansiaor.InstantiateBullet(firePos, direction, bulletPrefab);
    }
    private void OnDrawGizmosSelected()
    {
        if(firePos != null)
        {
            Gizmos.DrawWireSphere(firePos.position, 0.2f);
            Gizmos.color = Color.red;
            Vector3 direction = Vector3.zero;
            switch (spawnDirection)
            {
                case SpawnDirection.Right:
                    direction = firePos.right * 2;
                    break;
                case SpawnDirection.Left:
                    direction = -firePos.right * 2;
                    break;
            };
            Gizmos.DrawRay(transform.position, direction);
            Gizmos.DrawWireSphere(firePos.position + direction, 0.2f);
        }
        //void DrawParabola(Vector3 start, float speed, Vector3 direction, float gravity)
        //{
        //    Gizmos.color = Color.red;

        //    Vector3 previousPoint = start;
        //    float timeStep = 0.1f; 

        //    for (int i = 0; i <= resolution; i++)
        //    {
        //        float t = i * timeStep;

        //        Vector3 point = CalculateParabolaPoint(start, speed, direction, gravity, t);
        //        Gizmos.DrawLine(previousPoint, point);
        //        previousPoint = point;
        //    }
        //    Vector3 CalculateParabolaPoint(Vector3 start, float speed, Vector3 direction, float gravity, float time)
        //    {
        //        Vector3 normalizedDirection = direction.normalized;

        //        float x = start.x + normalizedDirection.x * speed * time;
        //        float y = start.y + normalizedDirection.y * speed * time + 0.5f * gravity * time * time;

        //        return new Vector3(x, y, start.z);
        //    }
        //}

    }
}
