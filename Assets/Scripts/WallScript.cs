using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using MonsterLove.StateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallScript : MonoBehaviour
{
    public StateMachine<States, StateDriverRunner> _fsm;
    
    public int row;
    public int column;
    public MarkUp markUp;
    
    public enum States
    {
        Init
    }
    private void Awake()
    {
        _fsm = new StateMachine<States, StateDriverRunner>(this);

    }

    private void Init_Enter()
    {
        row = Random.Range(0, 4);
        column = Random.Range(0, 4);

        while (markUp.fieldCells[row, column].isBusy != false)
        {
            row = Random.Range(0, 4);
            column = Random.Range(0, 4);
        }
        
        transform.position = markUp.fieldCells[row, column].globalCoordinates;
        markUp.fieldCells[row, column].isBusy = true;
        markUp.fieldCells[row, column].unitType = UnitType.Wall;
    }

    
    
    
    
    
    
    
    
    void Start()
    {
        _fsm.ChangeState(States.Init);
    }
    void Update()
    {
        
    }
}