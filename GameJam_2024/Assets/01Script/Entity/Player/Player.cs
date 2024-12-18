using System.Collections.Generic;
using UnityEngine;

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

    public float CurrentJumpCount { get; set; }
    public bool CanAirJump => CurrentJumpCount > 0;

    [HideInInspector] public PlayerInputCompo InputCompo { get; private set; }
    private EntityMover _mover;

    protected override void AfterInitialize()
    {
        base.AfterInitialize();
        InputCompo = GetComponent<PlayerInputCompo>();
        _mover = GetCompo<EntityMover>();

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
            float minDistance = hits[0].distance;
            RaycastHit2D firstHit = hits[0];
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.distance < minDistance)
                {
                    minDistance = hit.distance;
                    firstHit = hit;
                }
            }
            catchedBullet = firstHit.transform.GetComponent<BaseBullet>();
            return true;
        }
        catchedBullet = null;
        return false;
    }

    public void Dash()
    {
        _mover.KnockBack(GetMouseDirection(transform) * _dashPower, 0.7f);
    }

    public Vector3 GetMouseDirection(Transform trm)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        return (mousePosition - trm.position).normalized;
    }

    public void ResetJumpCount()
    {
        CurrentJumpCount = jumpCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BaseBullet bullet))
        {
            if (catchedBullet != null && catchedBullet.transform == bullet.transform) return;

            OnHitEvent?.Invoke();
            ChangeState("Hit");
        }
    }
}
