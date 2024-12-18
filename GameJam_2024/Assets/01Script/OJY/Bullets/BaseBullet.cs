//#define BULLETDEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseBullet : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected float speed = 5;
    [SerializeField] protected float deadForcePower = 21;
    //[SerializeField] protected float manualTurnSpeed = 5;

    [Header("Audio")]
    [SerializeField] protected AudioClipsSO onHoldAudio;
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
    }
    private void Update()
    {
        if (allowRotation)
        {
            transform.up = currentDirection;
            ClampRotation();
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
        Vector3 result = dir;// Vector3.RotateTowards(transform.up, dir, manualTurnSpeed * Time.deltaTime, 10);
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
        OnDeadForce();
        OnRelease();
    }
    public void Hold()
    {
        isHolded = true;

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
    private void OnTriggerEnter(Collider other)
    {
        //Destroy(gameObject);
    }
}
