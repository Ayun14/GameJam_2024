using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseBullet : MonoBehaviour
{
    protected bool allowMove = true;
    protected bool allowRotation = true;

    [SerializeField] protected float speed = 5;
    [SerializeField] protected float turnSpeed = 5;
    [SerializeField] protected float manualTurnSpeed = 30;
    protected Transform target;
    protected Vector3 currentDirection;

    protected Rigidbody2D rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
    }
    private void Update()
    {
        if (allowRotation)
        {
            transform.up = currentDirection;
            ClampRotation();
        }

        if (Input.GetKeyDown(KeyCode.K))
            Hold();
        if (Input.GetKeyDown(KeyCode.L))
            Release();
        if (Input.GetKey(KeyCode.N))
            Rotate(Vector3.up);
        if (Input.GetKey(KeyCode.M))
            Rotate(Vector3.right);
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
    public void Rotate(Vector3 dir)
    {

        ClampRotation();
    }
    public void Release()
    {
        OnRelease();
    }
    public void Hold()
    {
        rigid.velocity = Vector3.zero;
        OnHold();
    }
    protected virtual void OnRelease()
    {
        allowMove = true;
        //allowRotation = true;
    }
    protected virtual void OnHold()
    {
        allowRotation = false;
        allowMove = false;
    }
    public void Init(Vector3 initialDirection = default, Transform target = null)
    {
        this.target = target;
        currentDirection = initialDirection;
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
