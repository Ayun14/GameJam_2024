using UnityEngine;

public class PlayerAirState : EntityState
{
    protected Player _player;
    protected EntityMover _mover;

    public PlayerAirState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
        _player = entity as Player;
        _mover = entity.GetCompo<EntityMover>();
    }

    public override void Enter()
    {
        base.Enter();
        _mover.SetMoveSpeedMultiplier(0.7f);
        _player.InputCompo.OnJumpKeyEvent += HandleAirJump;
    }

    public override void Exit()
    {
        _mover.SetMoveSpeedMultiplier(1f);
        _player.InputCompo.OnJumpKeyEvent -= HandleAirJump;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        float xInput = _player.InputCompo.InputDirection.x;
        if (Mathf.Abs(xInput) > 0)
        {
            _mover.SetMovement(xInput);
        }
    }

    private void HandleAirJump()
    {
        if (_player.CanAirJump)
        {
            _player.ChangeState("Jump");
        }
    }
}
