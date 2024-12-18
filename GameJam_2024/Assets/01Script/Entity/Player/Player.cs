using System;
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

    public float CurrentJumpCount { get; set; }
    public bool CanAirJump => CurrentJumpCount > 0;

    [HideInInspector] public PlayerInputCompo InputCompo { get; private set; }

    protected override void AfterInitialize()
    {
        base.AfterInitialize();
        InputCompo = GetComponent<PlayerInputCompo>();

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

    public void ResetJumpCount()
    {
        CurrentJumpCount = jumpCount;
    }
}
