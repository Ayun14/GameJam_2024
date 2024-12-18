using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _mover.StopImmediately();
    }

    public override void Update()
    {
        base.Update();
        float xInput = _player.InputCompo.InputDirection.x;
        if (Mathf.Abs(xInput) > 0)
            _player.ChangeState("Move");
    }
}
