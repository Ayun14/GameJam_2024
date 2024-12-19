public class ParabolaBullet : BaseBullet
{
    private void Start()
    {
        rigid.velocity = currentDirection * speed;
    }
    protected override void Move()
    {
        //rigid.AddForce(currentDirection * speed, ForceMode2D.Force);
    }
}
