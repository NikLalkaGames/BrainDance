using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using MonsterLove.StateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour
{

    public StateMachine<States, StateDriverRunner> _fsm;
    
    public int roachNumber;
    public RoachNumber roach;
    public MarkUp markUp;
    public SimpleMovement player;
    public Animator anim;
    
    private int h;
    private int i;
    private int j;

    private int row;
    private int column;

    public int dance;

    public enum States
    {
        Init,
        Turn,
        Wait
    }
    

    private void Awake()
    {
        _fsm = new StateMachine<States, StateDriverRunner>(this);
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        dance = 0;
        _fsm.ChangeState(States.Init);
    }

    private void Update()
    {
        _fsm.Driver.Update.Invoke();
        
        if (markUp.fieldCells[row, column].globalCoordinates == player.nextPlayable)
        {
            player.transform.position = markUp.fieldCells[row, column].globalCoordinates;
            markUp.fieldCells[row, column].isBusy = true;
            markUp.fieldCells[row, column].unitType = UnitType.Player;
            gameObject.SetActive(false);
        }

        if (dance > 0)
        {
            anim.SetBool("dance", true);
        }
        else
        {
            anim.SetBool("dance", false);
        }
        
    }


    void Init_Enter()
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
        markUp.fieldCells[row, column].unitType = UnitType.Roach;

        _fsm.ChangeState(States.Wait);
    }

    void Turn_Enter()
    {
        StartCoroutine(Turn());
    }

    void Wait_Update()
    {
        if (roachNumber == roach.number)
        {
            _fsm.ChangeState(States.Turn);
        }
        
        
    }
    
    void Decision()
    {
        if (((markUp.fieldCells[row + 1, column].isBusy != true))
            || (markUp.fieldCells[row - 1, column].isBusy != true)
            || (markUp.fieldCells[row, column + 1].isBusy != true)
            || (markUp.fieldCells[row, column - 1].isBusy != true))
        {
            do
            {
                h = Random.Range(1, 5);

                switch (h)
                {
                    case 1:
                        j = 0;
                        i = 1;
                        break;
                    case 2:
                        j = 0;
                        i = -1;
                        break;
                    case 3:
                        i = 0;
                        j = 1;
                        break;
                    case 4:
                        i = 0;
                        j = -1;
                        break;

                }

            } while ((markUp.fieldCells[row + i, column + j].unitType != UnitType.None) && (markUp.fieldCells[row + i, column + j].unitType != UnitType.Goal));
        }
        else
        {
            StopCoroutine(Turn());
        }

        markUp.fieldCells[row, column].isBusy = false;
        markUp.fieldCells[row, column].unitType = UnitType.None;

        markUp.fieldCells[row + i, column + j].isBusy = true;
        markUp.fieldCells[row + i, column + j].unitType = UnitType.Roach;
        
        row = row + i;
        column = column + j;
        StartCoroutine(Lerp());
        
    }
    
    IEnumerator Turn()
    {
        if (dance == 0)
        {
            yield return new WaitForSeconds(1.5f);
            Decision();
            yield return new WaitForSeconds(0.5f);
            roach.number++;
            _fsm.ChangeState(States.Wait);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            roach.number++;
            dance--;
            _fsm.ChangeState(States.Wait);
        }
        
    }

    IEnumerator Lerp()
    {
        while (transform.position != markUp.fieldCells[row, column].globalCoordinates)
        {
            transform.position = Vector3.Lerp(transform.position,
                markUp.fieldCells[row, column].globalCoordinates, 0.05f);
            yield return null;
        }
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
        markUp.fieldCells[row, column].unitType = UnitType.Roach;

    }
    private void OnDisable()
    {
        markUp.fieldCells[row, column].isBusy = false;
        markUp.fieldCells[row, column].unitType = UnitType.None;
    }
}
