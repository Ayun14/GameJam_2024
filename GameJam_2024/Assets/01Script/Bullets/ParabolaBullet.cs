public class ParabolaBullet : BaseBullet
{
    //private void Start()
    //{
    //}
    public override void ApplyAdditional()
    {
        rigid.velocity = currentDirection * speed;
    }
    protected override void Move()
    {
        //rigid.AddForce(currentDirection * speed, ForceMode2D.Force);
    }
}
