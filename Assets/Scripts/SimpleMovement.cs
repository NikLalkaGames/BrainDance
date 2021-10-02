using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] private float unitSize;

    private Transform _transform;
    
    void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveStep(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveStep(Vector3.back);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveStep(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            MoveStep(Vector3.forward);
        }
    }

    private void MoveStep(Vector3 movement)
    {
        _transform.Translate(movement * unitSize);
    }
    
}
