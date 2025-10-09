using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] Column;
        private int _size = 0;

        public Row(int size)
        {
            CreateRow(size);
        }
        
        public void CreateRow(int size)
        {
            _size = size;
            Column = new bool[size];
            ClearRow();
        }
        
        public void ClearRow()
        {
            for (int i = 0; i < _size; i++)
            {
                Column[i] = false;
            }
        }
    }
    
    public int Columns = 0;
    public int Rows = 0;
    public Row[] Board;

    public void Clear()
    {
        for (var i = 0; i < Rows; i++)
        {
            Board[i].ClearRow();
        }
    }
    
    public void CreateNewBoard()
    {
        Board = new Row[Rows];
        for (int i = 0; i < Rows; i++)
        {
            Board[i] = new Row(Columns);
        }
    }
}
