using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    private EntityRenderer _renderer;

    public PlayerJumpState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
        _renderer = entity.GetCompo<EntityRenderer>();
    }

    public override void Enter()
    {
        base.Enter();
        Vector2 jumpPower = new Vector2(0, _player.JumpPower);

        _renderer.Jump(_player.jumpCountParam, _player.CurrentJumpCount--);
        _mover.StopImmediately(true);
        _mover.AddForceToEntity(jumpPower);
        _mover.OnMovement += HandleVelocityChange;
    }

    public override void Exit()
    {
        _mover.OnMovement -= HandleVelocityChange;
        base.Exit();
    }

    private void HandleVelocityChange(Vector2 velocity)
    {
        if (velocity.y < 0)
            _player.ChangeState("Fall");
    }
}
