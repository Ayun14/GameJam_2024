using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullet : BaseBullet
{
    [SerializeField] protected float turnSpeed = 5;
    [SerializeField] protected Transform target;
    private void Start()
    {
        target = default;
    }
    protected override void Move()
    {
        Vector3 targetPos = Vector3.zero;
        if (target != null) targetPos = target.position;
        else Debug.LogError("can't find target");

        Vector3 targetDir = targetPos - transform.position;
        targetDir.Normalize();
        Debug.DrawRay(transform.position, targetDir, Color.blue);

        float angle = Vector3.Angle(currentDirection, targetDir);
        float angleCalc = angle / 55;
        angleCalc = Mathf.Lerp(0.05f, 1, angleCalc);

        float maxRadiansDelta = angleCalc * Time.fixedDeltaTime * turnSpeed;
        const float maxMaginuteDelta = 10;

        //UI_DebugPlayer.Instance.GetList[0].text = angleCalc.ToString();
        //UI_DebugPlayer.Instance.GetList[1].text = angle.ToString();

        Debug.DrawRay(transform.position, currentDirection, Color.red);
        if(allowRotation)
            currentDirection = Vector3.RotateTowards(currentDirection, targetDir, maxRadiansDelta, maxMaginuteDelta);
        if(allowMove)
            rigid.velocity = currentDirection * speed;
    }
}
