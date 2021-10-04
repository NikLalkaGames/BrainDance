using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoachNumber : MonoBehaviour
{
    public int number;
    private bool f;
    void Start()
    {
    }
    
    void FixedUpdate()
    {
        if (number == 5)
        {
            number = 1;
        }
    }
    
}
