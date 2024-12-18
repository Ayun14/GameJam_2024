using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Unity.VisualScripting;

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
    [HideInInspector] public Transform arrowTrm;
    [HideInInspector] public Transform catchedBullet;

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
        arrowTrm = transform.Find("Arrow").GetComponent<Transform>();
        arrowTrm.gameObject.SetActive(false);

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
            catchedBullet = firstHit.transform;
            return true;
        }
        catchedBullet = null;
        return false;
    }

    public void Dash()
    {
        _mover.AddForceToEntity(arrowTrm.right.normalized * _dashPower);
    }

    public void ResetJumpCount()
    {
        CurrentJumpCount = jumpCount;
    }
}
