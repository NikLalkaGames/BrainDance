using System;
using System.Collections;
using Data;
using MonsterLove.StateMachine;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private TargetTransform targetNovel;
    
    [SerializeField] private TargetTransform targetField;

    private Vector3 _targetNovelPosition;
    
    private Quaternion _targetNovelRotation;

    private Vector3 _targetFieldPosition;

    private Quaternion _targetFieldRotation;

    [SerializeField] private float movementSpeed;

    [SerializeField] private float rotationSpeed;

    private Func<IEnumerator> _movementBehaviour;

    public enum States
    {
        CameraFixed,
        NovelMovement,
        FieldMovement
    }

    private StateMachine<States, StateDriverRunner> _fsm;
    
    
    private void Awake()
    {
        _transform = transform;

        _targetNovelPosition = targetNovel.targetTransform.position;
        _targetNovelRotation = targetNovel.targetTransform.rotation;
        
        _targetFieldPosition = targetField.targetTransform.position;
        _targetFieldRotation = targetField.targetTransform.rotation;

        _fsm = new StateMachine<States, StateDriverRunner>(this);
        _fsm.ChangeState(States.CameraFixed);
    }

    private void CameraFixed_Enter()
    {
        Debug.Log("Enter Camera fixed state");
    }
    
    private void CameraFixed_Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            _fsm.ChangeState(States.NovelMovement);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _fsm.ChangeState(States.FieldMovement);
        }
    }

    private void NovelMovement_Enter()
    {
        Debug.Log("Enter Novel movement state");
    }

    private void NovelMovement_Update()
    {
        if (Helper.Reached(_transform.position, _targetNovelPosition) && 
               Helper.Reached(_transform.rotation, _targetNovelRotation))
        {
            _fsm.ChangeState(States.CameraFixed);
        }
        
        _transform.position = Vector3.Lerp(_transform.position, _targetNovelPosition, Time.fixedDeltaTime * movementSpeed);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, _targetNovelRotation, Time.fixedDeltaTime * rotationSpeed);
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            _fsm.ChangeState(States.FieldMovement);
        }
    }
    
    private void FieldMovement_Enter()
    {
        Debug.Log("Enter Field movement state");
    }

    private void FieldMovement_Update()
    {
        if (Helper.Reached(_transform.position, _targetFieldPosition) && 
               Helper.Reached(_transform.rotation, _targetFieldRotation))
        {
            _fsm.ChangeState(States.CameraFixed);
        }
        
        _transform.position = Vector3.Lerp(_transform.position, _targetFieldPosition, Time.fixedDeltaTime * movementSpeed);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, _targetFieldRotation, Time.fixedDeltaTime * rotationSpeed);
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            _fsm.ChangeState(States.NovelMovement);
        }
    }

    private void Update()
    {
        _fsm.Driver.Update.Invoke();
    }
}
