using System;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] private float unitSize;

    private Transform _transform;
    public enum States
    {
        Init,
        ActionSelection,
        EffectSelection,
        EnemyTurn
    }
    
    private StateMachine<States, StateDriverRunner> _fsm;
    
    private void Awake()
    {
        _fsm = new StateMachine<States, StateDriverRunner>(this);
        _fsm.ChangeState(States.Init);
    }

    void Init_Enter()
    {
        _fsm.ChangeState(States.ActionSelection);
    }

    void Init_Update()
    {
        
    }

    void ActionSelection_Enter()
    {
        
    }
    
    void ActionSelection_Update()
    {
        
    }
    
    void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        _fsm.Driver.Update.Invoke();
    }
    
    
}
