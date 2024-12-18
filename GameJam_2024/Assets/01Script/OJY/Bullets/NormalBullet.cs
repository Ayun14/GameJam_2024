using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BaseBullet
{
    protected override void Move()
    {
        if(allowMove)
            rigid.velocity = currentDirection * speed;
    }
}
