using System;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    private float _currentTime = 0f;
    private float _moveSoundDelay = 0.3f;

    public PlayerMoveState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _currentTime = 0;
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

        MoveSoundPlay();
    }

    private void MoveSoundPlay()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _moveSoundDelay)
        {
            _currentTime = 0;
            SoundController.Instance.PlaySFX(8);
        }
    }
}
