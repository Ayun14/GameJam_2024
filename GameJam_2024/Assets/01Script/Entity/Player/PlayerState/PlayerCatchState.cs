using UnityEngine;

public class PlayerCatchState : PlayerAirState
{
    public PlayerCatchState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.InputCompo.OnMouseUpEvent += HandleEndCatch;

        Time.timeScale = 0f;
        if (_player.catchedBullet != null)
        {
            _player.catchedBullet.Hold();
            _player.arrowTrm.position = _player.catchedBullet.transform.position;
            _player.arrowTrm.gameObject.SetActive(true);
        }
    }

    public override void Update()
    {
        _player.catchedBullet.Rotate(_player.GetMouseDirection(_player.arrowTrm));
    }

    public override void Exit()
    {
        // 대쉬 하기
        if (_player.catchedBullet != null)
        {
            _player.catchedBullet.Release();
            _player.transform.position = _player.catchedBullet.transform.position;
            _player.Dash();
        }

        _player.ResetJumpCount();

        _player.InputCompo.OnMouseUpEvent -= HandleEndCatch;
        base.Exit();
    }

    private void HandleEndCatch()
    {
        _player.arrowTrm.gameObject.SetActive(false);
        Time.timeScale = 1f;

        _player.ChangeState("Fall");
    }
}
