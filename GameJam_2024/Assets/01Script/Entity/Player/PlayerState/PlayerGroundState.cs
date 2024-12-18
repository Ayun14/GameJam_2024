public class PlayerGroundState : EntityState
{
    protected Player _player;
    protected EntityMover _mover;

    public PlayerGroundState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
        _player = entity as Player;
        _mover = entity.GetCompo<EntityMover>();
    }

    public override void Enter()
    {
        base.Enter();
        _player.InputCompo.OnJumpKeyEvent += HandleJump;
    }

    public override void Exit()
    {
        _player.InputCompo.OnJumpKeyEvent -= HandleJump;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_mover.IsGroundDetected() == false && _mover.CanManualMove)
        {
            _player.ChangeState("Fall");
        }
    }

    private void HandleJump()
    {
        _player.ChangeState("Jump");
    }
}
