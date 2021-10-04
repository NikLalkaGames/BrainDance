using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using Data;
using MonsterLove.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class SimpleMovement : MonoBehaviour
{
    #region @FIELDS@




    [SerializeField] private float unitSize;
    public StateMachine<States, StateDriverRunner> _fsm;
    private Transform _transform;
    private Animator anim;
    public Activation active;
    
    
    public enum States
    {
        Init, //Init Phase
        ActionSelection, //Card Selection Phase
        EffectSelection, //Card's Effect Phase
        Goal,
        AiTurn,

        Stun //Wall Stun State
    }

    //FIELD
    public int row;
    public int column;
    public int rowDest;
    public int columnDest;

    public RoachNumber roach;
    public Vector3 nextPlayable;
    
    public int roachNumber;

    //OTHER
    public MarkUp markUp;
    
    //BOOLS
    public bool changeStateToAction; //EFFECT SELECT
    public bool stuck; //STUN STATE BOOL
    public bool goalReached;
    public bool disable;
    private bool dirSuccess;
    public bool choose;
    public bool dance;
    public bool dod;
    public bool dod1;
    public bool dod2;
    public bool can;
    public bool restart;
    
    
    public string action;

    public List<Vector3> positions;
    
    #endregion


    private int direction;




    #region @STATES@


    void Init_Enter()
    {
        //GENERATION PROCESS
        row = Random.Range(1, 8) + 6;
        column = Random.Range(1, 8) + 6;

        while (markUp.fieldCells[row, column].isBusy != false)
        {
            row = Random.Range(1, 8) + 6;
            column = Random.Range(1, 8) + 6;
        }
        
        transform.position = markUp.fieldCells[row, column].globalCoordinates;
        markUp.fieldCells[row, column].isBusy = true;
        markUp.fieldCells[row, column].unitType = UnitType.Player;


        _fsm.ChangeState(States.AiTurn);

    }


    void AiTurn_Enter()
    {
        roach.number++;
    }
    void AiTurn_Update()
    {
        if (roachNumber == roach.number)
        {
            _fsm.ChangeState(States.ActionSelection);
        }
    }

    void ActionSelection_Enter()
    {
        //CELL AFTER-TURN UPDATE
        markUp.fieldCells[row, column].isBusy = true;
        markUp.fieldCells[row, column].unitType = UnitType.Player;

        Debug.Log("ACTION");

        dod = true;
        dod1 = true;
        dod2 = true;
    }

    void ActionSelection_Update()
    {

        if (can == true)
        {
            can = false;
            _fsm.ChangeState(States.EffectSelection);
        }
    }




    void EffectSelection_Enter()
    {
        
        Debug.Log("Enter EffectSelection");
    }

    void EffectSelection_Update()
    {
        if (restart == true)
        {
            restart = false;
            active.active = true;
            _fsm.ChangeState(States.AiTurn);
        }
        
        if (dance == true)
        {
            dance = false;
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Capsules");
                foreach (GameObject go in gos)
                {
                    go.GetComponent<AI>().dance = 2;
                }
                _fsm.ChangeState(States.AiTurn);
        }
        
        //AFTER DIRECTION SELECT
        if (choose == true)
        {
            choose = false;
            _fsm.ChangeState(States.AiTurn);
        }
        
        if (changeStateToAction == true)
        {
            markUp.fieldCells[row, column].isBusy = false;
            markUp.fieldCells[row, column].unitType = UnitType.None;
            
            
            row = rowDest;
            column = columnDest;
            
            changeStateToAction = false;
            StartCoroutine(SuccesfulLerp(rowDest, columnDest));


            
            markUp.fieldCells[row, column].isBusy = true;
            markUp.fieldCells[row, column].unitType = UnitType.Player;
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

    }




    void Stun_Enter()
    {
        markUp.fieldCells[row, column].isBusy = false;
        markUp.fieldCells[row, column].unitType = UnitType.None;
        
        row = rowDest;
        column = columnDest;
        
        StartCoroutine(Lerp(rowDest, columnDest));
        
        Debug.Log("STUNED");
        
        anim.SetTrigger("stun");
        StartCoroutine("Timer");

    }




    void Goal_Enter()
    {
        StartCoroutine(SuccesfulLerpGoal(rowDest, columnDest));
        anim.SetTrigger("disappearAnim");
        
    }

    void Goal_Update()
    {

    }

    
    #endregion


    #region @METHODS@


    private void Awake()
    {
        _fsm = new StateMachine<States, StateDriverRunner>(this);
        anim = GetComponent<Animator>();
        
    }

    private void OnEnable()
    {
        //GENERATION PROCESS
        row = Random.Range(1, 8) + 6;
        column = Random.Range(1, 8) + 6;

        while (markUp.fieldCells[row, column].isBusy != false)
        {
            row = Random.Range(1, 8) + 6;
            column = Random.Range(1, 8) + 6;
        }
        
        transform.position = markUp.fieldCells[row, column].globalCoordinates;
        markUp.fieldCells[row, column].isBusy = true;
        markUp.fieldCells[row, column].unitType = UnitType.Player;

    }

    private void OnDisable()
    {
        markUp.fieldCells[row, column].isBusy = false;
        markUp.fieldCells[row, column].unitType = UnitType.None;
    }
    
    public DialogueText dialogueText;
    
    void Start()
    {
        _fsm.ChangeState(States.Init);
        _transform = transform;
        // dialogueText.ParseTextFile();
    }

    private void Update()
    {
        _fsm.Driver.Update.Invoke();

        if (disable == true)
        {
            gameObject.SetActive(false);
        }
    }

    /*IEnumerator FailedLerp()
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
    }*/

    IEnumerator SuccesfulLerp(int i, int j)
    {
        while (transform.position != markUp.fieldCells[i, j].globalCoordinates)
        {
            transform.position = Vector3.Lerp(transform.position,
                markUp.fieldCells[i, j].globalCoordinates, 0.05f);
            yield return null;
            _fsm.ChangeState(States.AiTurn);
        }

    }
    
    IEnumerator SuccesfulLerpGoal(int i, int j)
    {
        while (transform.position != markUp.fieldCells[i, j].globalCoordinates)
        {
            transform.position = Vector3.Lerp(transform.position,
                markUp.fieldCells[i, j].globalCoordinates, 0.05f);
            yield return null;
        }

    }

    IEnumerator Lerp(int i, int j)
    {
        while (transform.position != markUp.fieldCells[i, j].globalCoordinates)
        {
            transform.position = Vector3.Lerp(transform.position,
                markUp.fieldCells[i, j].globalCoordinates, 0.05f);
            yield return null;
        }
    }

    IEnumerator Timer()
        {
            stuck = false;
            yield return new WaitForSeconds(3.0f);
            _fsm.ChangeState(States.AiTurn);
        }
    
    #endregion
}
