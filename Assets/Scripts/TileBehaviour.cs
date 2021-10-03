using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public MarkUp markUp;
    public SimpleMovement player;
    
    
    
    public int row;
    public int column;
    
    
    void Start()
    {
        transform.position = markUp.fieldCells[row, column].globalCoordinates;
        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
    }

    
    private void OnMouseEnter()
    {
        if (player._fsm.State == SimpleMovement.States.EffectSelection)
        {
            if ((markUp.fieldCells[row + 1, column].unitType == UnitType.Player)
                || (markUp.fieldCells[row - 1, column].unitType == UnitType.Player)
                || (markUp.fieldCells[row, column + 1].unitType == UnitType.Player)
                || (markUp.fieldCells[row, column - 1].unitType == UnitType.Player))
            {
                GetComponent<MeshRenderer>().enabled = true;
            }
        }
        Debug.Log("IS BUSY:" + markUp.fieldCells[row,column].isBusy);
        Debug.Log("TYPE:" + markUp.fieldCells[row,column].unitType);
    }

    private void OnMouseDown()
    {
        if ((GetComponent<MeshRenderer>().enabled == true) && (markUp.fieldCells[row, column].unitType == UnitType.None))
        {
            player.slerp = markUp.fieldCells[row,column].globalCoordinates;
            markUp.fieldCells[row, column].isBusy = true;
            markUp.fieldCells[row, column].unitType = UnitType.Player;
            
            markUp.fieldCells[player.row, player.column].isBusy = false;
            markUp.fieldCells[player.row, player.column].unitType = UnitType.None;
            
            player.row = row;
            player.column = column;

            player.changeStateToAction = true;
        }
        else if ((GetComponent<MeshRenderer>().enabled == true) && (markUp.fieldCells[row, column].unitType == UnitType.Wall))
        {
            player.nextrow = row;
            player.nextcolumn = column;
            player.stuck = true;
        }
        else if ((GetComponent<MeshRenderer>().enabled == true) && (markUp.fieldCells[row, column].unitType == UnitType.Roach))
        {
            player.nextrow = row;
            player.nextcolumn = column;
            player.stuck = true;
        }
        else if ((GetComponent<MeshRenderer>().enabled == true) &&
                 (markUp.fieldCells[row, column].unitType == UnitType.Goal))
        {
            player.row = row;
            player.column = column;
            player.goalReached = true;
        }
    }

    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
