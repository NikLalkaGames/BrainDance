using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MonsterLove.StateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Action2 : MonoBehaviour
{

    public SimpleMovement player;
    public StateMachine<States, StateDriverRunner> _fsm;
    private Animator anim;

    public bool SS;
    
    public Action TT;
    public Action1 TTT;

    private int action;
    public bool can;
    public enum States
    {
        Init,
        Turn,
        Wait
    }


    void Wait_Update()
    {
        if (player.dod2 == true)
        {
            player.dod2 = false;
            
                action = Random.Range(1, 3);

                if (action == 1)
                {
                    anim.SetTrigger("Action1");
                    StartCoroutine(Wait());
                    _fsm.ChangeState(States.Turn);

                }

                if (action == 2)
                {
                    anim.SetTrigger("Action2");
                    StartCoroutine(Wait());
                    _fsm.ChangeState(States.Turn);
                }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10f);
        can = true;
    }

    private void OnMouseDown()
    {
        if (can == true)
        {
            can = false;
            TT.can = false;
            TTT.can = false;
            if (action == 1)
            {
                player.action = "Dance";
                player.dance = true;
            }
            if (action == 2)
            {
                player.action = "Restart";
                player.restart = true;
            }

            player.can = true;
            
            _fsm.ChangeState(States.Wait);
            TT._fsm.ChangeState(Action.States.Wait);
            TTT._fsm.ChangeState(Action1.States.Wait);
            
        }
    }


    private void Awake()
    {
        
    }

    void Start()
    {
        _fsm = new StateMachine<States, StateDriverRunner>(this);
        anim = GetComponent<Animator>();
        _fsm.ChangeState(States.Wait);
    }


    void Update()
    {
        _fsm.Driver.Update.Invoke();
    }
}
