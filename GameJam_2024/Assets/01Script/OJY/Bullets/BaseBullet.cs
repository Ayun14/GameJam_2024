// #define BULLETDEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseBullet : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected float speed = 5;
    [SerializeField] protected float deadForcePower = 21;

    //[Header("Rotation")]
    //[SerializeField] private float _rotationSpeed;

    [Header("OnHold")]
    [SerializeField] protected AudioClipsSO onHoldAudio;

    [Header("OnRelease")]
    [SerializeField] protected GameObject onReleaseEffect;
    [SerializeField] protected Transform onReleaseEffectTransform;
    [SerializeField] protected AudioClipsSO onReleaseAudio;


#if BULLETDEBUG
    [Header("Debug")]
    [SerializeField] protected Vector3 playerArrowDir;
#endif

    protected bool allowMove = true;
    protected bool allowRotation = true;
    protected bool isHolded;
    protected Vector3 currentDirection;

    protected AudioSource audioSource;
    protected Rigidbody2D rigid;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
        transform.up = currentDirection;
    }
    private void Update()
    {
        if (allowRotation)
        {
            transform.up = currentDirection;
            ClampRotation();
        }
        Vector3 min = BulletBoundary.Instance.Min;
        Vector3 max = BulletBoundary.Instance.Max;
        if (transform.position.x < min.x || transform.position.y < min.y || 
            transform.position.x > max.x || transform.position.y > max.y)
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
        isHolded = false;

        //allowMove = true;
        onReleaseAudio.Play(onReleaseAudio.SelectedAudioClip, audioSource);
        Instantiate(onReleaseEffect, onReleaseEffectTransform.position, onReleaseEffectTransform.rotation, onReleaseEffectTransform);
        OnDeadForce();
        OnRelease();
    }
    public void Hold()
    {
        isHolded = true;
        if (TryGetComponent(out Collider2D currentCollider))
            currentCollider.enabled = false;
        else Debug.LogWarning("bullet has no collider");

        allowRotation = false;
        allowMove = false;
        rigid.velocity = Vector3.zero;
        onHoldAudio.Play(onHoldAudio.SelectedAudioClip, audioSource);
        OnHold();
    }
    protected virtual void OnRelease()
    {
    }
    protected virtual void OnHold()
    {
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
        if(!Mathf.Approximately(speed, 0))
            this.speed = speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
   
}
