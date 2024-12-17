using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private float speed = 5;
    [SerializeField] private Transform target;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
    }
    private void FixedUpdate()
    {
        Vector3 result = target.position - transform.position;
        result.Normalize();
        Debug.DrawRay(transform.position, result, Color.red);
        result *= speed;
        rigid.velocity = result;
    }
    public void Init(Transform target = null)
    {
        this.target = target;
    }
}
