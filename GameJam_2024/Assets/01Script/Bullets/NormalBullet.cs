public class NormalBullet : BaseBullet
{
    protected override void Move()
    {
        if (allowMove)
            rigid.velocity = currentDirection * speed;
    }
}
