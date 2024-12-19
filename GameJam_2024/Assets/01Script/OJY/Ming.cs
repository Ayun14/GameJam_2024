using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ming : MonoBehaviour
{
    [SerializeField] private Transform followTransform;
    private void Update()
    {
        transform.position = followTransform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BaseBullet bullet))
        {
            bullet.OnHighlightEnter();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BaseBullet bullet))
        {
            bullet.OnHighlightExit();
        }
    }
}
