using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject _squarePrefab;
    [SerializeField] private ShapeStorage _shapeStorage;
    [SerializeField] private float _squareScale; // tỉ lệ của ô

    public List<GameObject> GridSquares = new();
    private Vector2 _offset;
    private LineIndicator _lineIndicator;

    private void OnEnable()
    {
        GameEventManager.CheckShapeDown += CheckIfShapeCanBePlaced;
        GameEventManager.ReturnPoolObjects += ReturnPoolObjects;
    }

    private void OnDisable()
    {
        GameEventManager.CheckShapeDown -= CheckIfShapeCanBePlaced;
        GameEventManager.ReturnPoolObjects -= ReturnPoolObjects;
    }

    private void Start()
    {
        //CreateGrid();
        _lineIndicator = GetComponent<LineIndicator>();
        CreateGridBoard();
    }
    
    private void ReturnPoolObjects()
    {
        foreach (var square in GridSquares)
        {
            square.GetComponent<GridSquare>().ReturnPool();
        }
    }

    private void CreateGridBoard()
    {
        for (int i = 0; i < 64; i++)
        {
            GameObject square = ObjectPooler.Instance.SpawnFromPool("GridSquare", transform.position);
            square.transform.SetParent(this.transform);
            square.transform.localScale = Vector3.one * _squareScale;
            GridSquares.Add(square);

            GridSquare gridSquare = square.GetComponent<GridSquare>();
            gridSquare.SquareIndex = i;
            GameController.Instance.CurrentState[i] = gridSquare;
        }
    }

    private void CheckIfShapeCanBePlaced()
    {
        var squareIndexes = new List<int>();

        foreach (var square in GridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.Selected && !gridSquare.SquareOccupied)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
            }
        }

        var currentSelectedShape = _shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return;
        if (currentSelectedShape.TotalSquareNumber == squareIndexes.Count)
        {
            var shapeSquares = currentSelectedShape.GetComponentsInChildren<ShapeSquare>(true)
                .Where(sq => sq.gameObject.activeSelf).ToList();

            // Gán Type và icon từ ShapeSquare vào GridSquare
            for (int i = 0; i < squareIndexes.Count; i++)
            {
                var gridSquare = GridSquares[squareIndexes[i]].GetComponent<GridSquare>();
                var shapeSquare = shapeSquares[i];

                // Gán Type và icon
                gridSquare.SquareType = (GridSquare.Type)shapeSquare.SquareType;
                gridSquare.PlaceShapeOnBoard(
                    haveIcon: shapeSquare.SquareType != ShapeSquare.Type.None,
                    iconIndex: shapeSquare.GetIconIndex(),
                    color: shapeSquare.Color
                );
            }

            var shapeLeft = 0;

            foreach (var shape in _shapeStorage.ShapeList)
            {
                if (shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
                {
                    shapeLeft++;
                }
            }

            if (shapeLeft == 0)
            {
                GameController.Instance.PrepareNextSpawnShapes2();
            }
            else
            {
                AudioManager.LightFeedback();
                GameEventManager.SetShapeInactive?.Invoke();
            }

            CheckIfAnyLineIsCompleted();
        }
        else
        {
            GameEventManager.MoveShapeToStartPosition?.Invoke();
        }
    }

    private void CheckIfAnyLineIsCompleted()
    {
        List<int[]> columns = new();
        List<int[]> rows = new();

        foreach (var column in _lineIndicator.ColumnIndexes)
        {
            columns.Add(_lineIndicator.GetVerticalLine(column));
        }

        for (var row = 0; row < 8; row++)
        {
            int[] rowData = new int[8];
            for (var index = 0; index < 8; index++)
            {
                rowData[index] = _lineIndicator.LineData[row, index];
            }
            rows.Add(rowData);
        }

        // Kiểm tra tất cả
        var (completedColumns, columnSquares) = CheckIfSquaresAreCompleted(columns);
        var (completedRows, rowSquares) = CheckIfSquaresAreCompleted(rows);

        // Gộp danh sách
        List<int> allCompletedSquares = columnSquares.Concat(rowSquares).Distinct().ToList();

        // Xóa các ô
        foreach (var squareIndex in allCompletedSquares)
        {
            var comp = GridSquares[squareIndex].GetComponent<GridSquare>();
            comp.Deactivate();
            comp.ClearOccupied();
        }

        // Tính tổng số dòng hoàn thành
        int completedLines = completedColumns + completedRows;

        // Bonus ăn nhiều hàng cùng lúc
        if (completedLines > 2)
        {
            // Play bonus animation.
        }

#if UNITY_EDITOR
        // if (allCompletedSquares.Count > 0)
        // {
        //     Debug.LogError("Các ô hoàn thành: " + string.Join(", ", allCompletedSquares));
        // }
#endif

        if (allCompletedSquares.Count > 0)
        {
            AudioManager.PlaySound("Goal");
            GameController.Instance.ItemExecuted(allCompletedSquares);
        }

        CheckIfPlayerHasLost();
    }

    private (int, List<int>) CheckIfSquaresAreCompleted(List<int[]> data)
    {
        List<int> completedSquares = new();
        int linesCompleted = 0;

        foreach (var line in data)
        {
            bool isLineCompleted = line.All(squareIndex => 
                GridSquares[squareIndex].GetComponent<GridSquare>().SquareOccupied
            );

            if (isLineCompleted)
            {
                completedSquares.AddRange(line);
                linesCompleted++;
            }
        }

        return (linesCompleted, completedSquares);
    }

    private void CheckIfPlayerHasLost()
    {
        var validShapes = 0;
        foreach (var shape in _shapeStorage.ShapeList)
        {
            var isShapeActive = shape.IsAnyOfShapeSquareActive();
            if (CheckIfShapeCanBePlaceOnGrid(shape) && isShapeActive)
            {
                shape.ActivateShape();
                validShapes++;
            }
        }

        // Logic game over
        if (validShapes == 0)
        {
            GameUIManager.Instance.ShowRevivePanel();
        }
    }

    private bool CheckIfShapeCanBePlaceOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.CurrentShapeData;
        var shapeColumns = currentShapeData.Columns;
        var shapeRows = currentShapeData.Rows;

        List<int> originalShapeFilledUpSquares = new List<int>();
        var squareIndex = 0;

        for (var row = 0; row < shapeRows; row++)
        {
            for (var column = 0; column < shapeColumns; column++)
            {
                if (currentShapeData.Board[row].Column[column])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }

                squareIndex++;
            }
        }

        var squareList = GetAllSquaresCombination(shapeColumns, shapeRows);

        bool canBePlaced = false;

        foreach (var number in squareList)
        {
            bool shapeCanBePlacedOnTheBoard = true;
            foreach (var squareIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = GridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if (comp.SquareOccupied)
                {
                    shapeCanBePlacedOnTheBoard = false;
                }
            }

            if (shapeCanBePlacedOnTheBoard)
            {
                canBePlaced = true;
            }
        }

        return canBePlaced;
    }

    private List<int[]> GetAllSquaresCombination(int columns, int rows)
    {
        var squareList = new List<int[]>();

        for (int lastRowIndex = 0; lastRowIndex <= 8 - rows; lastRowIndex++)
        {
            for (int lastColmnIndex = 0; lastColmnIndex <= 8 - columns; lastColmnIndex++)
            {
                var rowData = new List<int>();
                for (int row = lastRowIndex; row < lastRowIndex + rows; row++)
                {
                    for (int column = lastColmnIndex; column < lastColmnIndex + columns; column++)
                    {
                        rowData.Add(_lineIndicator.LineData[row, column]);
                    }
                }
                squareList.Add(rowData.ToArray());
            }
        }

        return squareList;
    }

    // private void CreateGrid()
    // {
    //     SpawnGridSquares();
    //     PositionGridSquares();
    // }
    //
    // private void SpawnGridSquares()
    // {
    //     for (int i = 0; i < _columnSize * _rowSize; i++)
    //     {
    //         GameObject square = Instantiate(_squarePrefab, transform);
    //         square.transform.localScale = Vector3.one * _squareScale;
    //         _gridSquares.Add(square);
    //     }
    // }
    //
    // private void PositionGridSquares()
    // {
    //     if (_gridSquares.Count == 0) return;
    //
    //     RectTransform squareRect = _squarePrefab.GetComponent<RectTransform>();
    //
    //     _offset.x = squareRect.rect.width * _squareScale + _squareOffset;
    //     _offset.y = squareRect.rect.height * _squareScale + _squareOffset;
    //
    //     for (int row = 0; row < _rowSize; row++)
    //     {
    //         for (int column = 0; column < _columnSize; column++)
    //         {
    //             int index = row * _columnSize + column;
    //
    //             if (index >= _gridSquares.Count) break;
    //
    //             Vector2 position = new Vector2(
    //                 _startPosition.x + column * (_offset.x + _squareOffset),
    //                 _startPosition.y - row * (_offset.y + _squareOffset)
    //             );
    //
    //             RectTransform squareTransform = _gridSquares[index].GetComponent<RectTransform>();
    //             squareTransform.anchoredPosition = position;
    //             squareTransform.localPosition = new Vector3(position.x, position.y, 0);
    //         }
    //     }
    // }
}