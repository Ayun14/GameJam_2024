using UnityEngine;

public class PlayerCatchState : PlayerAirState
{
    private CameraShakeController _shaker;

    public PlayerCatchState(Entity entity, AnimatorParamSO stateParam) : base(entity, stateParam)
    {
        _shaker = entity.GetCompo<CameraShakeController>();
    }

    public override void Enter()
    {
        base.Enter();

        _player.InputCompo.OnMouseUpEvent += HandleEndCatch;

        Time.timeScale = 0f;
        if (_player.catchedBullet != null)
        {
            _player.catchedBullet.Hold();
            _player.arrow.transform.position = _player.catchedBullet.transform.position;
            _player.arrow.ArrowSetActive(true, 0.1f);
        }
    }

    public override void Update()
    {
        if (_player.catchedBullet != null)
        {
            _player.catchedBullet.Rotate(_player.GetMouseDirection(_player.arrow.transform));
        }
    }

    public override void Exit()
    {
        // 대쉬 하기
        if (_player.catchedBullet != null)
        {
            // Particle
            _player.dashParticle.transform.position = _player.catchedBullet.transform.position;
            _player.dashParticle.Play();

            // Sound
            SoundController.Instance.PlaySFX(2);

            _player.transform.position = _player.catchedBullet.transform.position;
            _player.Dash();
            _player.catchedBullet.Release();

            _shaker.CameraShake();
            _player.ResetJumpCount();

            _player.catchedBullet = null;
        }

        _player.InputCompo.OnMouseUpEvent -= HandleEndCatch;
        base.Exit();
    }

    private void HandleEndCatch()
    {
        _player.arrow.ArrowSetActive(false, 0.1f);
        Time.timeScale = 1f;

        _player.ChangeState("Fall");
    }
}
