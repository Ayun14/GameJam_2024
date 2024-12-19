// #define BULLETDEBUG
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseBullet : MonoBehaviour
{
    private static class Cache
    {
        public static readonly LayerMask playerLayer = LayerMask.GetMask("Player");
    }
    [SerializeField] private string _bulletPoolName;

    [Header("General")]
    [SerializeField] protected float speed = 5;

    [SerializeField] protected float deadForcePower = 21;
    [SerializeField] private Transform onDeadTransform;
    [SerializeField] private SpriteRenderer visualSprite;
    //[SerializeField] private MiniPool miniPool;

    //[Header("Rotation")]
    //[SerializeField] private float _rotationSpeed;


    //[Header("OnHold")]
    //[SerializeField] protected AudioClip onHoldAudio;

    //[Header("OnRelease")]
    //[SerializeField] protected GameObject onReleaseEffect;
    //[SerializeField] protected Transform onReleaseEffectTransform;
    //[SerializeField] protected AudioClip onReleaseAudio;


#if BULLETDEBUG
    [Header("Debug")]
    [SerializeField] protected Vector3 playerArrowDir;
#endif

    protected bool allowMove = true;
    protected bool allowRotation = true;
    protected bool isHolded;
    public bool IsHolded => isHolded;
    protected Vector3 currentDirection;

    protected AudioSource audioSource;
    protected Rigidbody2D rigid;
    private void Awake()
    {
        SoundController.Instance.PlaySFX(7, 0.1f);
        audioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
        transform.up = currentDirection;
        //miniPool.Init(prefab, 10);
    }
    private void OnEnable()
    {
        //for pool
        allowMove = true;
        allowRotation = true;
        isHolded = false;
        rigid.velocity = Vector3.zero;//?
        //need to call this.Init(); every pool
    }
    private void Update()
    {
        if (allowRotation)
        {
            transform.up = currentDirection;
            ClampRotation();
        }
        //Vector3 min = BulletBoundary.Instance.Min;
        //Vector3 max = BulletBoundary.Instance.Max;
        //if (transform.position.x < min.x || transform.position.y < min.y || 
        //    transform.position.x > max.x || transform.position.y > max.y)
        if (!transform.IsPositionValid())
        {
            Destroy(gameObject);
        }
#if BULLETDEBUG

        if (Input.GetKeyDown(KeyCode.K))
            Hold();
        if (Input.GetKeyDown(KeyCode.L))
            Release();
        if (Input.GetKeyDown(KeyCode.N))
            Rotate(playerArrowDir);
#endif
    }
    private void ClampRotation()
    {
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.x = 0;
        eulerAngles.y = 0;
        transform.rotation = Quaternion.Euler(eulerAngles);
    }
    private void FixedUpdate()
    {
        Move();
    }
    protected abstract void Move();
    /// <summary>
    /// call this function when rotating player arrow
    /// </summary>
    /// <param name="dir">dir of player arrow</param>
    public void Rotate(Vector3 dir)
    {
        Vector3 result = dir;
        result *= -1;
        transform.up = result;
        currentDirection = result;
        ClampRotation();
    }
    public void Release()
    {
        //allowMove = true;
        //onReleaseAudio.Play(onReleaseAudio.SelectedAudioClip, audioSource);
        //Instantiate(onReleaseEffect, onReleaseEffectTransform.position, onReleaseEffectTransform.rotation, onReleaseEffectTransform);
        OnDeadForce();
        OnRelease();
    }
    public void Hold()
    {
        isHolded = true;
        if (TryGetComponent(out Collider2D currentCollider))
            currentCollider.excludeLayers = Cache.playerLayer;
        else Debug.LogWarning("bullet has no collider");

        allowRotation = false;
        allowMove = false;
        rigid.velocity = Vector3.zero;
        SoundController.Instance.PlaySFX(1);
        //onHoldAudio.Play(onHoldAudio.SelectedAudioClip, audioSource);
        OnHold();
    }
    protected virtual void OnRelease()
    {
    }
    protected virtual void OnHold()
    {
    }
    public void OnHighlightEnter()
    {
        //print("ent");
        Color32 highlightEnterColor = new Color32(255, 153, 242, 255);
        visualSprite.color = highlightEnterColor;
    }
    public void OnHighlightExit()
    {
        //print("exit");
        Color32 highlightEnterColor = new Color32(255, 255, 255, 255);
        visualSprite.color = highlightEnterColor;
    }

    protected virtual void OnDeadForce()
    {
        rigid.gravityScale = 1;
        Debug.DrawRay(transform.position, currentDirection * deadForcePower, Color.red, 5);
        rigid.AddForce(currentDirection * deadForcePower, ForceMode2D.Impulse);
    }
    public void Init(Vector3 initialDirection = default, float speed = 0)
    {
        currentDirection = initialDirection;
        if (!Mathf.Approximately(speed, 0))
            this.speed = speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 9)
            OnDead();
    }
    public void Kill()
    {
        OnDead();
    }

    private void OnDead()
    {
        PoolManager.Instance.Pop("BulletDeadEffect", onDeadTransform.position, Quaternion.identity);
        PoolManager.Instance.Push(_bulletPoolName, gameObject);
    }

}
