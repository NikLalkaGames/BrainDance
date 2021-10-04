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
        row = Random.Range(1, 8) + 6;
        column = Random.Range(1, 8) + 6;

        while (markUp.fieldCells[row, column].isBusy != false)
        {
            row = Random.Range(1, 8) + 6;
            column = Random.Range(1, 8) + 6;
        }
        
        transform.position = markUp.fieldCells[row, column].globalCoordinates;
        markUp.fieldCells[row, column].isBusy = true;
        markUp.fieldCells[row, column].unitType = UnitType.Wall;
    }


    private void OnEnable()
    {
        row = Random.Range(1, 8) + 6;
        column = Random.Range(1, 8) + 6;

        while (markUp.fieldCells[row, column].isBusy != false)
        {
            row = Random.Range(1, 8) + 6;
            column = Random.Range(1, 8) + 6;
        }
        
        transform.position = markUp.fieldCells[row, column].globalCoordinates;
        markUp.fieldCells[row, column].isBusy = true;
        markUp.fieldCells[row, column].unitType = UnitType.Wall;
    }
    private void OnDisable()
    {
        markUp.fieldCells[row, column].isBusy = false;
        markUp.fieldCells[row, column].unitType = UnitType.None;
    }

    void Start()
    {
        _fsm.ChangeState(States.Init);
    }
    void Update()
    {
        
    }
}
