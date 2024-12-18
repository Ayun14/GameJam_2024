public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
    }

    public override void Update()
    {
        base.Update();
        if (_mover.IsGroundDetected())
        {
            _player.ResetJumpCount();
            _player.ChangeState("Idle");
        }
    }
}
