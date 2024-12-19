using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Player : Entity
{
    [Header("Stat")]
    [SerializeField] private float jumpPower;
    public float JumpPower => jumpPower;
    [SerializeField] private float jumpCount;
    public float JumpCount => jumpCount;

    [Header("FSM")]
    public List<StateSO> stateList;
    public AnimatorParamSO jumpCountParam;
    private StateMachine _stateMachine;

    [Header("Catch")]
    [SerializeField] private float _dashPower;
    [SerializeField] private float _catchRadius;
    [SerializeField] private LayerMask _whatIsBullet;
    [HideInInspector] public Arrow arrow;
    [HideInInspector] public BaseBullet catchedBullet;

    [Header("Ending Timeline")]
    [SerializeField] private Transform _endingTrm;

    public float CurrentJumpCount { get; set; }
    public bool CanAirJump => CurrentJumpCount > 0;

    [HideInInspector] public PlayerInputCompo InputCompo { get; private set; }
    private EntityMover _mover;

    protected override void AfterInitialize()
    {
        base.AfterInitialize();
        InputCompo = GetComponent<PlayerInputCompo>();
        _mover = GetCompo<EntityMover>();

        ResetJumpCount();

        // Arrow
        arrow = transform.Find("Arrow").GetComponent<Arrow>();
        arrow.transform.gameObject.SetActive(false);

        // FSM
        _stateMachine = new StateMachine(stateList, this);
        _stateMachine.InitStateMachine("Idle");

        GetCompo<EntityAnimatorTrigger>().OnAnimationEnd += HandleAnimationEnd;
    }

    private void OnDestroy()
    {
        GetCompo<EntityAnimatorTrigger>().OnAnimationEnd -= HandleAnimationEnd;
    }

    private void HandleAnimationEnd()
    {
        _stateMachine.CurrentState.AnimationEndTrigger();
    }

    private void Update()
    {
        _stateMachine.UpdateFSM();
    }

    public void ChangeState(string newState)
        => _stateMachine.ChangeStateMachine(newState);

    public bool IsCanCatch()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _catchRadius, Vector3.up, 0f, _whatIsBullet);
        if (hits.Length > 0)
        {
            float minDistance = Mathf.Infinity;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            for (int i = 0; i < hits.Length; i++)
            {
                BaseBullet bullet = hits[i].transform.GetComponent<BaseBullet>();
                float distance = Vector2.Distance(mousePos, hits[i].point);
                if (distance <= minDistance && bullet.IsHolded == false) //hits[i].distance
                {
                    minDistance = distance;//hits[i].distance;
                    catchedBullet = bullet;
                }
            }
        }
        return catchedBullet != null;
    }

    public void Dash()
    {
        _mover.KnockBack(GetMouseDirection(transform) * _dashPower);
    }

    public Vector3 GetMouseDirection(Transform trm)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        return (mousePosition - trm.position).normalized;
    }

    public void SpawnDashEffect()
    {
        GameObject go = PoolManager.Instance.Pop("DashEffect", catchedBullet.transform.position, Quaternion.identity);
        if (go.TryGetComponent(out DashEffect effect))
            effect.DashEffectPlay();
    }

    public void ResetJumpCount()
    {
        CurrentJumpCount = jumpCount;
        GetCompo<EntityRenderer>().Jump(jumpCountParam, CurrentJumpCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BaseBullet bullet))
        {
            if (catchedBullet != null && catchedBullet.transform == bullet.transform) return;

            OnHitEvent?.Invoke();
            SoundController.Instance.PlaySFX(4);
            ChangeState("Hit");
        }
    }

    #region Timeline

    public void StartEndingTimeline(PlayableDirector timeline)
    {
        InputCompo.isEnding = true;
        StartCoroutine(EndingTimelineRoutine(timeline));
    }

    private IEnumerator EndingTimelineRoutine(PlayableDirector timeline)
    {
        yield return new WaitUntil(() => _mover.IsGroundDetected());
        ChangeState("Idle");
        timeline.Play();
    }

    public void MoveEndingPos()
    {
        transform.DOMoveX(_endingTrm.position.x, 3.5f);
    }

    #endregion
}
