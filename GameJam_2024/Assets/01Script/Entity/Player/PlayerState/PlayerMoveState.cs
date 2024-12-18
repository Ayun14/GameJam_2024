using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
    }

    public override void Update()
    {
        base.Update();

        float xInput = _player.InputCompo.InputDirection.x;
        _mover.SetMovement(xInput);

        if (Mathf.Approximately(xInput, 0))
        {
            _player.ChangeState("Idle");
        }
    }
}
