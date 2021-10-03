using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using Data;
using MonsterLove.StateMachine;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class SimpleMovement : MonoBehaviour
{
    #region @FIELDS@




    [SerializeField] private float unitSize;
    public StateMachine<States, StateDriverRunner> _fsm;
    private Transform _transform;
    private Animator anim;


    public enum States
    {
        Init, //Init Phase
        ActionSelection, //Card Selection Phase
        EffectSelection, //Card's Effect Phase
        EnemyTurn, //Other Roaches Turn Phase
        Goal,
        PreAI,

        Stun //Wall Stun State
    }

    //FIELD
    public int row;
    public int column;
    public int nextrow;
    public int nextcolumn;

    public int roachNumber;
    private int currentNumber;

    //OTHER
    public MarkUp markUp;
    private FieldCell lastCellPos;
    public Vector3 slerp;


    //BOOLS
    public bool changeStateToAction; //EFFECT SELECT
    public bool stuck; //STUN STATE BOOL
    public bool goalReached;
    public bool disable;
    public bool aiTurn;
    private bool dirSuccess;


    #endregion


    private int direction;




    #region @STATES@


    void Init_Enter()
    {
        //GENERATION PROCESS
        row = Random.Range(0, 4);
        column = Random.Range(0, 4);

        while (markUp.fieldCells[row, column].isBusy != false)
        {
            row = Random.Range(0, 4);
            column = Random.Range(0, 4);
        }



        transform.position = markUp.fieldCells[row, column].globalCoordinates;
        markUp.fieldCells[row, column].isBusy = true;
        if (roachNumber == currentNumber)
        {
            markUp.fieldCells[row, column].unitType = UnitType.Player;
        }
        else
        {
            markUp.fieldCells[row, column].unitType = UnitType.Roach;
        }

        //Debug.Log(transform.position);



        lastCellPos = markUp.fieldCells[row, column];

        _fsm.ChangeState(States.PreAI);

    }
    

    void PreAI_Enter()
    {
        if (roachNumber == currentNumber)
        {
            _fsm.ChangeState(States.ActionSelection);
        }
        else
        {
            _fsm.ChangeState(States.EnemyTurn);
        }
    }


    void ActionSelection_Enter()
    {
        //CELL AFTER-TURN UPDATE
        markUp.fieldCells[row, column].isBusy = true;
        markUp.fieldCells[row, column].unitType = UnitType.Player;

        Debug.Log("ACTION");
    }

    void ActionSelection_Update()
    {
        //CARD'S EFFECT TRIGGER
        if (Input.GetKeyDown(KeyCode.A))
        {
            _fsm.ChangeState(States.EffectSelection);
        }

    }




    void EffectSelection_Enter()
    {

        Debug.Log("Enter EffectSelection");
    }

    void EffectSelection_Update()
    {
        //AFTER DIRECTION SELECT
        if (changeStateToAction == true)
        {
            changeStateToAction = false;
            StartCoroutine(SuccesfulLerp(0, 0));
        }

        if (stuck == true)
        {
            _fsm.ChangeState(States.Stun);
        }

        if (goalReached == true)
        {
            _fsm.ChangeState(States.Goal);
        }
    }

    void EffectSelection_Exit()
    {
        //CLEAN PREVIOUS CELL FROM PLAYER'S DATA
        markUp.fieldCells[row, column].isBusy = false;
        markUp.fieldCells[row, column].unitType = UnitType.None;
        aiTurn = true;
    }




    void Stun_Enter()
    {
        // StartCoroutine("FailedLerp");
        Debug.Log("STUNED");
        StartCoroutine("Timer");

    }




    void Goal_Enter()
    {
        StartCoroutine(SuccesfulLerp(0, 0));
        anim.SetTrigger("disappearAnim");

    }

    void Goal_Update()
    {
        if (disable == true)
        {
            gameObject.SetActive(false);
        }
    }



    void EnemyTurn_Enter()
    {
        direction = Random.Range(1, 4);
    }

    void EnemyTurn_Update()
    {
        while (dirSuccess != true)
        {
            if ((direction == 1) && (markUp.fieldCells[row + 1, column].isBusy == false))
            {
                StartCoroutine(SuccesfulLerp(1, 0));
                dirSuccess = true;
            }
            else if ((direction == 2) && (markUp.fieldCells[row - 1, column].isBusy == false))
            {
                StartCoroutine(SuccesfulLerp(-1, 0));
                dirSuccess = true;
            }
            else if ((direction == 3) && (markUp.fieldCells[row, column + 1].isBusy == false))
            {
                StartCoroutine(SuccesfulLerp(0, 1));
                dirSuccess = true;
            }
            else if ((direction == 4) && (markUp.fieldCells[row, column - 1].isBusy == false))
            {
                StartCoroutine(SuccesfulLerp(0, -1));
                dirSuccess = true;
            }
            else if ((markUp.fieldCells[row, column - 1].isBusy == true) &&
                     (markUp.fieldCells[row, column + 1].isBusy == true) &&
                     (markUp.fieldCells[row + 1, column].isBusy == true) &&
                     (markUp.fieldCells[row - 1, column].isBusy == true))
            {
                _fsm.ChangeState(States.PreAI);
            }
            else
            {
                direction = Random.Range(1, 4);
            }

            _fsm.ChangeState(States.PreAI);
        }
    }

    #endregion




    #region @METHODS@


    private void Awake()
    {
        _fsm = new StateMachine<States, StateDriverRunner>(this);
        anim = GetComponent<Animator>();

        roachNumber = GetComponentInParent<RoachNumber>().number;
    }


    public DialogueText dialogueText;
    
    void Start()
    {
        _fsm.ChangeState(States.Init);
        _transform = transform;
        dialogueText.ParseTextFile();
    }

    private void Update()
    {
        _fsm.Driver.Update.Invoke();
    }

    IEnumerator FailedLerp()
    {
        while (transform.position != markUp.fieldCells[nextrow, nextcolumn].globalCoordinates / 2)
        {
            transform.position = Vector3.Lerp(transform.position,
                markUp.fieldCells[nextrow, nextcolumn].globalCoordinates / 2,
                0.03f);
        }

        while (transform.position != markUp.fieldCells[row, column].globalCoordinates)
        {
            transform.position =
                Vector3.Lerp(transform.position, markUp.fieldCells[row, column].globalCoordinates, 0.01f);
            yield return null;
        }
    }

    IEnumerator SuccesfulLerp(int i, int j)
    {
        while (transform.position != markUp.fieldCells[row + i, column + j].globalCoordinates)
        {
            transform.position = Vector3.Lerp(transform.position,
                markUp.fieldCells[row + i, column + j].globalCoordinates, 0.05f);
            yield return null;
            _fsm.ChangeState(States.ActionSelection);
        }

    }

    IEnumerator SuccesfulLerpAI(int i, int j)
    {
        while (transform.position != markUp.fieldCells[row + i, column + j].globalCoordinates)
        {
            transform.position = Vector3.Lerp(transform.position,
                markUp.fieldCells[row + i, column + j].globalCoordinates, 0.05f);
            yield return null;
            _fsm.ChangeState(States.ActionSelection);
        }

        IEnumerator Timer()
        {
            stuck = false;
            yield return new WaitForSeconds(1.0f);
            _fsm.ChangeState(States.EnemyTurn);
        }

        #endregion
    }
}
