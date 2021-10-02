using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] private float unitSize;

    private Transform _transform;

    private Vector3 _movement;
    
    void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        if ((_movement.z = Input.GetAxis("Vertical")) != 0)
        {
            
        }
        else if ((_movement.x = Input.GetAxis("Horizontal")) != 0)
        {
            
        }
    }
}
