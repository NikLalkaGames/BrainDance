using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    
    public int row;
    public int column;
    public MarkUp markUp;
    
    public StateMachine<States, StateDriverRunner> _fsm;
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
        markUp.fieldCells[row, column].unitType = UnitType.Goal;
    }

    private void Init_Update()
    {
        
    }
    
    void Start()
    {
        _fsm.ChangeState(States.Init);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}