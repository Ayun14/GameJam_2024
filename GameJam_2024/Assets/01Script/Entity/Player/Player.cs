using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Stat")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpCount;

    [Header("FSM")]
    public List<StateSO> stateList;
    private StateMachine _stateMachine;

    public float CurrentJumpCount { get; set; }
    public bool CanAirJump => CurrentJumpCount > 0;

    private PlayerInputCompo _inputCompo;
    //private PlayerAttackCompo _atkCompo;

    protected override void AfterInitialize()
    {
        base.AfterInitialize();
        _stateMachine = new StateMachine(stateList, this);
        _stateMachine.InitStateMachine("IDLE");

        _inputCompo = GetCompo<PlayerInputCompo>();
        //_atkCompo = GetCompo<PlayerAttackCompo>();
        //_inputCompo.On += HandleAttackKeyEvent;
        GetCompo<EntityAnimatorTrigger>().OnAnimationEnd += HandleAnimationEnd;
    }

    private void OnDestroy()
    {
        //_inputCompo.On += HandleAttackKeyEvent;
        GetCompo<EntityAnimatorTrigger>().OnAnimationEnd -= HandleAnimationEnd;
    }

    private void HandleOnJumpKeyEvent()
    {
        throw new NotImplementedException();
    }

    private void HandleAttackKeyEvent()
    {
        //if (_stateMachine.CurrentState == _stateMachine.GetState("ATTACK")) return;
        //if (_atkCompo.AttemptAttack())
        //{
        //    ChangeState("ATTACK");
        //}
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
