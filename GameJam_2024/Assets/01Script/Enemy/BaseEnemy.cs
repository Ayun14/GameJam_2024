using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private enum SpawnDirection
    {
        None,
        Right,
        Left,
        Down,
        Up
    }
    [Header("General")]
    [SerializeField] private Transform visualTransform;

    [Header("Bullet Settings")]
    [SerializeField] private BaseBulletInstantiatorSO instansiaor;
    [SerializeField] private BaseBullet bulletPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private SpawnDirection spawnDirection;
    [SerializeField] private float speed = 5;

    [Header("Shoot Settings")]
    [SerializeField] private float repeatDuration = 2.5f;
    private float timer = 0;

    [Header("Animation Settings")]
    [SerializeField] private AnimatorParamSO idleParam;
    [SerializeField] private AnimatorParamSO attackParam;
    [SerializeField] private float animationAttackDuration;
    private Animator enemyAnimator;
    private bool isPlayed;

    //[SerializeField] private bool parabolaPreview;
    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        if (spawnDirection == SpawnDirection.Left)
        {
            Vector3 result = visualTransform.eulerAngles;
            result.y = 180;
            visualTransform.eulerAngles = result;
        }
        if (spawnDirection == SpawnDirection.Up)
        {
            Vector3 result = visualTransform.eulerAngles;
            result.z = 180;
            visualTransform.eulerAngles = result;
        }
    }
    private void Update()
    {
        if (Game.GameFinished) return;
        timer += Time.deltaTime;
        if (timer >= repeatDuration - animationAttackDuration && !isPlayed)
        {
            //print("playanimation" + timer);
            enemyAnimator.Play(attackParam.hashValue, -1, 0);
            isPlayed = true;
        }
        if (timer >= repeatDuration)
        {
            //print("fire" + timer);
            timer = 0;
            FireBullet();
            enemyAnimator.Play(idleParam.hashValue, -1, 0);
            isPlayed = false;
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //    FireBullet();
    }
    private void FireBullet()
    {
        //if enemy position is out of bounds than return;
        if (!transform.IsPositionValid()) return;

        Vector3 direction = Vector3.zero;
        switch (spawnDirection)
        {
            case SpawnDirection.Right:
                direction = firePos.right;
                break;
            case SpawnDirection.Left:
                direction = -firePos.right;
                break;
            case SpawnDirection.Down:
                direction = -firePos.up;
                break;
            case SpawnDirection.Up:
                direction = firePos.up;
                break;
        };
        if (gameObject.name == "Slime (24)")
            Debug.Log("SHot");
        instansiaor.InstantiateBullet(firePos, direction, bulletPrefab, speed);
    }
    private void OnDrawGizmosSelected()
    {
        if (firePos != null)
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
                case SpawnDirection.Down:
                    direction = -firePos.up * 2;
                    break;
                case SpawnDirection.Up:
                    direction = firePos.up * 2;
                    break;
            };
            Gizmos.DrawRay(firePos.position, direction);
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
