using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Vector2 jumpPower = new Vector2(0, _player.JumpPower);

        _renderer.Jump(_player.jumpCountParam, _player.CurrentJumpCount--);
        _mover.StopImmediately(true);
        _mover.AddForceToEntity(jumpPower);
        _mover.OnMovement += HandleVelocityChange;

        // Sound
        int playIdx = 5;
        if (_player.CurrentJumpCount == 0)
            playIdx = 6;
        SoundController.Instance.PlaySFX(playIdx);
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
