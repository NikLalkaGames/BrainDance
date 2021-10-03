using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MarkUp : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 currentCellPos;
    public int cellRow;
    public int cellColumn;
    public FieldCell[,] fieldCells;
    
    void Awake()
    {
        fieldCells = new FieldCell[cellRow, cellColumn];
        fieldCells[0, 0] = new FieldCell()
        {
            globalCoordinates = currentCellPos
        };
        float initialxPos = currentCellPos.x;
        for (int i = 0; i < cellRow; i++)
        {
            for (int j = 0; j < cellColumn; j++)
            {
                fieldCells[i, j] = new FieldCell();
                fieldCells[i, j].globalCoordinates = currentCellPos;

                fieldCells[i, j].isBusy = false;
                fieldCells[i, j].unitType = UnitType.None;
                currentCellPos.x += 2;
                
                //Debug.Log(fieldCells[i, j].globalCoordinates);
            }

            currentCellPos.x = initialxPos;
            currentCellPos.z -= 2;
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
