using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MarkUp : MonoBehaviour
{
    // Start is called before the first frame update

    public Field field;

    public Vector3 currentCellPos;
    
    
    void Awake()
    {
        var fieldCells = field.fieldCells;

        fieldCells[0, 0] = new FieldCell()
        {
            globalCoordinates = currentCellPos
        };
        float initialxPos = currentCellPos.x;
        for (int i = 0; i < field.cellRow; i++)
        {
            for (int j = 0; j < field.cellColumn; j++)
            {
                fieldCells[i, j] = new FieldCell();
                fieldCells[i, j].globalCoordinates = currentCellPos;

                fieldCells[i, j].isBusy = false;
                currentCellPos.x += 2;
                Debug.Log(fieldCells[i, j].globalCoordinates);
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
