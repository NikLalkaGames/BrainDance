using UnityEngine;




    [CreateAssetMenu(menuName = "Field/FieldCell", order = 0)]
    public class Field: ScriptableObject
    {
        public FieldCell[,] fieldCells = new FieldCell[7,7];
        
        public int cellRow;
        public int cellColumn;

    }