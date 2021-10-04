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

    private string dir;
    
    void Start()
    {
        row = row + 6;
        column = column + 6;
        transform.position = markUp.fieldCells[row, column].globalCoordinates;
        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
    }

    
    private void OnMouseEnter()
    {
        if (player._fsm.State == SimpleMovement.States.EffectSelection)
        {
            if (player.action == "MoveOneCell")
            {
                if ((markUp.fieldCells[row + 1, column].unitType == UnitType.Player)
                    || (markUp.fieldCells[row - 1, column].unitType == UnitType.Player)
                    || (markUp.fieldCells[row, column + 1].unitType == UnitType.Player)
                    || (markUp.fieldCells[row, column - 1].unitType == UnitType.Player))
                {
                    GetComponent<MeshRenderer>().enabled = true;
                }
            }

            if (player.action == "MoveTwoCells")
            {
                if (markUp.fieldCells[row + 2, column].unitType == UnitType.Player)
                {
                    dir = "left";
                    GetComponent<MeshRenderer>().enabled = true;
                }
                else if (markUp.fieldCells[row - 2, column].unitType == UnitType.Player)
                {
                    dir = "right";
                    GetComponent<MeshRenderer>().enabled = true;
                }
                else if (markUp.fieldCells[row, column + 2].unitType == UnitType.Player)
                {
                    dir = "down";
                    GetComponent<MeshRenderer>().enabled = true;
                }
                else if (markUp.fieldCells[row, column - 2].unitType == UnitType.Player)
                {
                    dir = "up";
                    GetComponent<MeshRenderer>().enabled = true;
                }
            }

            if (player.action == "RunTwoCells")
            {
                if (markUp.fieldCells[row + 2, column].unitType == UnitType.Player)
                {
                    dir = "left";
                    GetComponent<MeshRenderer>().enabled = true;
                }
                else if (markUp.fieldCells[row - 2, column].unitType == UnitType.Player)
                {
                    dir = "right";
                    GetComponent<MeshRenderer>().enabled = true;
                }
                else if (markUp.fieldCells[row, column + 2].unitType == UnitType.Player)
                {
                    dir = "down";
                    GetComponent<MeshRenderer>().enabled = true;
                }
                else if (markUp.fieldCells[row, column - 2].unitType == UnitType.Player)
                {
                    dir = "up";
                    GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }

        //Debug.Log("IS BUSY:" + markUp.fieldCells[row,column].isBusy);
        //Debug.Log("TYPE:" + markUp.fieldCells[row,column].unitType);
    }

    private void OnMouseDown()
    {
        if ((player.action == "MoveOneCell") && (GetComponent<MeshRenderer>().enabled == true))
        {
            MoveCellAction();
        }

        if ((player.action == "MoveTwoCells") && (GetComponent<MeshRenderer>().enabled == true))
        {
            if (dir == "right") 
            {
                MoveMultipleCellsAction(-1, 0);
            }
            else if (dir == "left") 
            {
                MoveMultipleCellsAction(1, 0);
            }
            else if (dir == "up") 
            {
                MoveMultipleCellsAction(0, -1);
            }
            else if (dir == "down") 
            {
                MoveMultipleCellsAction(0, 1);
            }
        }

        if ((player.action == "RunTwoCells") && (GetComponent<MeshRenderer>().enabled == true))
        {
            if (dir == "right")
            {
                RunMultipleCellsAction(-1, 0);
            }
            else if (dir == "left")
            {
                RunMultipleCellsAction(1, 0);
            }
            else if (dir == "up")
            {
                RunMultipleCellsAction(0, -1);
            }
            else if (dir == "down")
            {
                RunMultipleCellsAction(0, 1);
            }
        }
    }
    
    private void MoveCellAction()
    {
        if (markUp.fieldCells[row, column].unitType == UnitType.None)
        {
            player.rowDest = row;
            player.columnDest = column;
            player.changeStateToAction = true;
        }

        else if (markUp.fieldCells[row, column].unitType == UnitType.Wall)
        {
            player.choose = true;
        }
        else if (markUp.fieldCells[row, column].unitType == UnitType.Roach)
        {
            player.choose = true;
        }
        else if (markUp.fieldCells[row, column].unitType == UnitType.Goal)
        {
            player.rowDest = row;
            player.columnDest = column;
            player.goalReached = true;
        }
    }

    private void MoveMultipleCellsAction(int i, int j)
    {
        if (markUp.fieldCells[row + i, column + j].unitType == UnitType.None)
        {
            if (markUp.fieldCells[row, column].unitType == UnitType.None)
            {
                player.rowDest = row;
                player.columnDest = column;
                player.changeStateToAction = true;
            }

            else if (markUp.fieldCells[row, column].unitType == UnitType.Wall)
            {
                player.rowDest = row + i;
                player.columnDest = column + j;
                player.changeStateToAction = true;
            }
            else if (markUp.fieldCells[row, column].unitType == UnitType.Roach)
            {
                player.rowDest = row + i;
                player.columnDest = column + j;
                player.changeStateToAction = true;
            }
            else if (markUp.fieldCells[row, column].unitType == UnitType.Goal)
            {
                player.rowDest = row;
                player.columnDest = column;
                player.goalReached = true;
            }
        }
        else if (markUp.fieldCells[row + i, column + j].unitType == UnitType.Wall)
        {
            player.choose = true;
        }
        else if (markUp.fieldCells[row + i, column + j].unitType == UnitType.Roach)
        {
            player.choose = true;
        }
        else if (markUp.fieldCells[row + i, column + j].unitType == UnitType.Goal)
        {
            player.rowDest = row + i;
            player.columnDest = column + j;
            player.goalReached = true;
        }
    }
    
    private void RunMultipleCellsAction(int i, int j)
    {
        if (markUp.fieldCells[row + i, column + j].unitType == UnitType.None)
        {
            if (markUp.fieldCells[row, column].unitType == UnitType.None)
            {
                player.rowDest = row;
                player.columnDest = column;
                player.changeStateToAction = true;
            }

            else if (markUp.fieldCells[row, column].unitType == UnitType.Wall)
            {
                player.rowDest = row + i;
                player.columnDest = column + j;
                player.stuck = true;
            }
            else if (markUp.fieldCells[row, column].unitType == UnitType.Roach)
            {
                player.rowDest = row + i;
                player.columnDest = column + j;
                player.stuck = true;
            }
            else if (markUp.fieldCells[row, column].unitType == UnitType.Goal)
            {
                player.rowDest = row;
                player.columnDest = column;
                player.goalReached = true;
            }
        }
        else if (markUp.fieldCells[row + i, column + j].unitType == UnitType.Wall)
        {
            player.stuck = true;
        }
        else if (markUp.fieldCells[row + i, column + j].unitType == UnitType.Roach)
        {
            player.stuck = true;
        }
        else if (markUp.fieldCells[row + i, column + j].unitType == UnitType.Goal)
        {
            player.rowDest = row + i;
            player.columnDest = column + j;
            player.goalReached = true;
        }
    }
    
    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
