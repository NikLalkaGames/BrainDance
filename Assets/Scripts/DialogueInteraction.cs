using System;
using System.Collections.Generic;
using Data;
using MonsterLove.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DialogueInteraction : MonoBehaviour //, IPointerDownHandler
{
    private enum States
    {
        Messages,
        AnswerSelection
    }

    private int _dialogIndex;
    
    private int _selectionIndex;

    private int _selectedAnswer = -1;

    private int _backgroundIndex;
    

    [SerializeField] private List<DialogueText> straightDialogs;
    
    [SerializeField] private List<DialogueBranch> dialogueBranches; 
    
    [SerializeField] private List<AnswerSelection> answerSelection;

    [SerializeField] private List<Sprite> backgrounds;

    public Transform backTransform;


    public DialogueManager dialogueManager;
    
    public GameObject selectionWindow;      // bool selected

    private StateMachine<States, StateDriverRunner> _fsm;

    private void Awake()
    {
        _fsm = new StateMachine<States, StateDriverRunner>(this);
    }

    private void Start()
    {
        Debug.Log("Enter init state in dialogue interaction state machine");
        
        _dialogIndex = 0;
        _selectionIndex = 0;
        _backgroundIndex = 0;

        UpdateBackground(backgrounds[_backgroundIndex]);
        
        dialogueManager.LoadDialogToQueue(straightDialogs[_dialogIndex]);
        
        dialogueManager.ShowNextMessage();
        
        _fsm.ChangeState(States.Messages);
    }

    private void Messages_Enter()
    {
        Debug.Log("Enter messages state in dialogue interaction");
    }

    private void Messages_Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueManager.dialogueQueue.Count == 0)
            {
                if (straightDialogs[_dialogIndex].endsWithChoices)
                {
                    _fsm.ChangeState(States.AnswerSelection);
                }
                else
                {
                    _dialogIndex++;
                    dialogueManager.LoadDialogToQueue(straightDialogs[_dialogIndex]);

                    _backgroundIndex++;
                    UpdateBackground(backgrounds[_backgroundIndex]);
                }
                
            }


            dialogueManager.ShowNextMessage();
        }
    }

    private void AnswerSelection_Enter()
    {
        Debug.Log("AnswerSelection state of dialogue interaction");
        
        // set values of selection window
        selectionWindow.SetActive(true);
    }

    private void AnswerSelection_Update()
    {
        // emulating destination of some cell in the field
        if (Input.GetKeyDown(KeyCode.O))
        {
            _selectedAnswer = Random.Range(0, 2);
            _fsm.ChangeState(States.Messages);
        }
    }

    private void AnswerSelection_Exit()
    {
        selectionWindow.SetActive(false);

        dialogueManager.LoadDialogToQueue(dialogueBranches[_selectionIndex].variants[_selectedAnswer]);
        _selectionIndex++;
        
        _backgroundIndex++;
        UpdateBackground(backgrounds[_backgroundIndex]);
    }


    private void Update()
    {
        _fsm.Driver.Update.Invoke();
    }


    public void UpdateBackground(Sprite back)
    {
        backTransform.GetComponent<SpriteRenderer>().sprite = back;
    }
}