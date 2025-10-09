using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIndicator : MonoBehaviour
{
    public int[,] LineData = new int[8, 8]
    {
        { 0, 1, 2, 3, 4, 5, 6, 7 },
        { 8, 9, 10, 11, 12, 13, 14, 15 },
        { 16, 17, 18, 19, 20, 21, 22, 23 },
        { 24, 25, 26, 27, 28, 29, 30, 31 },
        { 32, 33, 34, 35, 36, 37, 38, 39 },
        { 40, 41, 42, 43, 44, 45, 46, 47 },
        { 48, 49, 50, 51, 52, 53, 54, 55 },
        { 56, 57, 58, 59, 60, 61, 62, 63 }
    };

    public int[,] SquareData = new int[8, 8]
    {
        { 0, 1, 2, 3, 4, 5, 6, 7 },
        { 8, 9, 10, 11, 12, 13, 14, 15 },
        { 16, 17, 18, 19, 20, 21, 22, 23 },
        { 24, 25, 26, 27, 28, 29, 30, 31 },
        { 32, 33, 34, 35, 36, 37, 38, 39 },
        { 40, 41, 42, 43, 44, 45, 46, 47 },
        { 48, 49, 50, 51, 52, 53, 54, 55 },
        { 56, 57, 58, 59, 60, 61, 62, 63 }
    };
    
    [HideInInspector]
    public int[] ColumnIndexes = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };

    private (int, int) GetSquarePosition(int squareIndex)
    {
        int rowPos = -1;
        int colPos = -1;
        
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (LineData[row, col] == squareIndex)
                {
                    rowPos = row;
                    colPos = col;
                }
            }
        }
        
        return (rowPos, colPos);
    }
    
    public int[] GetVerticalLine(int squareIndex)
    {
        int[] line = new int[8];
        var squarePositionCol = GetSquarePosition(squareIndex).Item2;
        
        for (int index = 0; index < 8; index++)
        {
            line[index] = LineData[index, squarePositionCol];
        }
        
        return line;
    }

    public int GetGridSquareIndex(int square)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (SquareData[row, col] == square)
                {
                    return row;
                }
            }
        }
        
        return -1;
    }
}
