public class PlayerHitState : EntityState
{
    private Player _player;
    private EntityMover _mover;

    public PlayerHitState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
        _player = entity as Player;
        _mover = entity.GetCompo<EntityMover>();
    }

    public override void Enter()
    {
        base.Enter();

        _mover.StopImmediately();
    }

    public override void Update()
    {
        base.Update();
        if (_mover.IsGroundDetected())
        {
            SoundController.Instance.PlaySFX(3);
            _player.ResetJumpCount();
            _player.ChangeState("Idle");
        }
    }
}
