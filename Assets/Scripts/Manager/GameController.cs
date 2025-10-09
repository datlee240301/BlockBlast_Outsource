using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : Singleton<GameController>
{
    public class BlockShape
    {
        public int[,] Shape { get; set; }
        public int TotalSquares { get; set; }
        public int Type { get; set; }

        public BlockShape(int[,] shape, int totalSquares, int type)
        {
            Shape = shape;
            TotalSquares = totalSquares;
            Type = type;
        }
    }

    [Header("========= References =========")] 
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private ShapeStorage _shapeStorage;

    [Header("========= Others =========")] public GridSquare[] CurrentState = new GridSquare[64];
    public List<int> WeaponIndexList = new();
    public List<int> SpecialIndexList = new();

    public BlockShape[] BlockShapes = new BlockShape[34]
    {
        new BlockShape(new int[,]
        {
            { 0, 0, 0 },
            { 0, 0, 0 },
            { 0, 0, 0 }
        }, 9, 0),

        new BlockShape(new int[,]
        {
            { 0, 0, 0 },
            { 0, 0, 0 },
        }, 6, 1),

        new BlockShape(new int[,]
        {
            { 0, 0 },
            { 0, 0 },
            { 0, 0 }
        }, 6, 2),

        new BlockShape(new int[,]
        {
            { 0, 1, 1 },
            { 0, 1, 1 },
            { 0, 0, 0 }
        }, 5, 3),

        new BlockShape(new int[,]
        {
            { 1, 1, 0 },
            { 1, 1, 0 },
            { 0, 0, 0 }
        }, 5, 4),

        new BlockShape(new int[,]
        {
            { 0, 0, 0 },
            { 0, 1, 1 },
            { 0, 1, 1 }
        }, 5, 5),

        new BlockShape(new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 0 },
            { 1, 1, 0 }
        }, 5, 6),

        new BlockShape(new int[,]
        {
            { 0, 0, 0 },
            { 0, 1, 1 },
        }, 4, 7),

        new BlockShape(new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 0 },
        }, 4, 8),

        new BlockShape(new int[,]
        {
            { 1, 1, 0 },
            { 0, 0, 0 },
        }, 4, 9),

        new BlockShape(new int[,]
        {
            { 0, 1, 1 },
            { 0, 0, 0 },
        }, 4, 10),

        new BlockShape(new int[,]
        {
            { 0, 1 },
            { 0, 1 },
            { 0, 0 }
        }, 4, 11),

        new BlockShape(new int[,]
        {
            { 0, 0 },
            { 0, 1 },
            { 0, 1 }
        }, 4, 12),

        new BlockShape(new int[,]
        {
            { 0, 0 },
            { 1, 0 },
            { 1, 0 }
        }, 4, 13),

        new BlockShape(new int[,]
        {
            { 1, 0 },
            { 1, 0 },
            { 0, 0 }
        }, 4, 14),

        new BlockShape(new int[,]
        {
            { 0, 1 },
            { 0, 0 },
            { 0, 1 }
        }, 4, 15),

        new BlockShape(new int[,]
        {
            { 1, 0 },
            { 0, 0 },
            { 1, 0 }
        }, 4, 16),

        new BlockShape(new int[,]
        {
            { 1, 0, 1 },
            { 0, 0, 0 },
        }, 4, 17),

        new BlockShape(new int[,]
        {
            { 0, 0, 0 },
            { 1, 0, 1 },
        }, 4, 18),

        new BlockShape(new int[,]
        {
            { 0, 0, 1 },
            { 1, 0, 0 },
        }, 4, 19),

        new BlockShape(new int[,]
        {
            { 1, 0, 0 },
            { 0, 0, 1 },
        }, 4, 20),

        new BlockShape(new int[,]
        {
            { 0, 1 },
            { 0, 0 },
            { 1, 0 }
        }, 4, 21),

        new BlockShape(new int[,]
        {
            { 1, 0 },
            { 0, 0 },
            { 0, 1 }
        }, 4, 22),

        new BlockShape(new int[,]
        {
            { 0, 0 },
            { 0, 0 },
        }, 4, 23),

        new BlockShape(new int[,]
        {
            { 0, 0, 0, 0 }
        }, 4, 24),

        new BlockShape(new int[,]
        {
            { 0 },
            { 0 },
            { 0 },
            { 0 }
        }, 4, 25),

        new BlockShape(new int[,]
        {
            { 0, 0 },
            { 0, 1 },
        }, 3, 26),

        new BlockShape(new int[,]
        {
            { 0, 0 },
            { 1, 0 },
        }, 3, 27),

        new BlockShape(new int[,]
        {
            { 1, 0 },
            { 0, 0 },
        }, 3, 28),

        new BlockShape(new int[,]
        {
            { 0, 1 },
            { 0, 0 },
        }, 3, 29),

        new BlockShape(new int[,]
        {
            { 0, 0, 0 }
        }, 3, 30),

        new BlockShape(new int[,]
        {
            { 0 },
            { 0 },
            { 0 }
        }, 3, 31),

        new BlockShape(new int[,]
        {
            { 0, 0 },
        }, 2, 32),

        new BlockShape(new int[,]
        {
            { 0 },
            { 0 }
        }, 2, 33)
    };

    private int _totalSquaresNeedToSpawn;
    private int[,] _tempMatrix = new int[8, 8];
    private readonly WaitForSeconds _time = new WaitForSeconds(0.33f);

    private void Start()
    {
        _enemySpawner.SpawnEnemy(0);
    }

    public void ItemExecuted(List<int> indexes)
    {
        foreach (var index in indexes)
        {
            if (CurrentState[index].SquareType == GridSquare.Type.Weapon)
            {
                switch (CurrentState[index].ItemType)
                {
                    case 0:
                        // Mũi tên đơn
                        ObjectPooler.Instance.SpawnFromPool("Arrow", CurrentState[index].transform.position);
                        break;
                    case 1:
                        // Mũi tên ba
                        StartCoroutine(SpawnTripleArrows(CurrentState[index].transform.position));
                        break;
                    case 2:
                        // Rocket
                        ObjectPooler.Instance.SpawnFromPool("Rocket", CurrentState[index].transform.position);
                        break;
                }
            }
            else if (CurrentState[index].SquareType == GridSquare.Type.Special)
            {
                switch (CurrentState[index].ItemType)
                {
                    case 3:
                        // Coin
                        GameEventManager.CoinCollected?.Invoke(1);
                        break;
                    case 4:
                        // Heart
                        var heart = ObjectPooler.Instance.SpawnFromPool("Heart", CurrentState[index].transform.position);
                        heart.GetComponent<Heart>().Move();
                        break;
                }
            }

            CurrentState[index].SquareType = GridSquare.Type.None;
        }
    }
    
    private IEnumerator SpawnTripleArrows(Vector3 position)
    {
        for (int i = 0; i < 3; i++)
        {
            ObjectPooler.Instance.SpawnFromPool("Arrow", position);
            yield return _time;
        }
    }

    #region ============================= New Logic =============================

    // public class InfoShape
    // {
    //     public int[,] Shape { get; set; }
    //     public int Type { get; set; }
    //     public int Row { get; set; }
    //     public int Col { get; set; }
    //
    //     public InfoShape(int[,] shape, int type, int row, int col)
    //     {
    //         Shape = shape;
    //         Type = type;
    //         Row = row;
    //         Col = col;
    //     }
    // }
    //
    // private List<InfoShape> _shapesFitList = new List<InfoShape>();
    private int _bestSum = -1;
    private int _sum = 0;
    private int _typeSpawn = -1;
    public List<int> TypeSpawn = new();
    // public bool ZeroSpaceEnough;
    [SerializeField] private int _col;
    [SerializeField] private int _row;
    // private bool _spaceLeft;
    //
    // public void PrepareNextSpawnShapes()
    // {
    //     TypeSpawn.Clear();
    //     ZeroSpaceEnough = false;
    //     _col = -1;
    //     _row = -1;
    //     GetCurrentBoard();
    //     GameEventManager.RequestNewShapes?.Invoke();
    // }
    //
    // private void GetCurrentBoard()
    // {
    //     for (int i = 0; i < CurrentState.Length; i++)
    //     {
    //         int row = i / 8; // Lấy chỉ số hàng (dòng)
    //         int col = i % 8; // Lấy chỉ số cột
    //
    //         _tempMatrix[row, col] = CurrentState[i].SquareOccupied ? 1 : 0; // 1: True, 0: False
    //     }
    //
    //     for (int i = 0; i < 3; i++)
    //     {
    //         CheckNextShape();
    //     }
    // }
    //
    // private void CheckNextShape()
    // {
    //     _shapesFitList.Clear();
    //     _bestSum = -1;
    //     _typeSpawn = -1;
    //     // Duyệt qua tất cả các khối trong danh sách
    //     foreach (var shapes in BlockShapes)
    //     {
    //         var shapeRows = shapes.Shape.GetLength(0);
    //         var shapeCols = shapes.Shape.GetLength(1);
    //
    //         for (int i = 0; i <= 8 - shapeRows; i++)
    //         {
    //             for (int j = 0; j <= 8 - shapeCols; j++)
    //             {
    //                 if (DoesShapeFit(shapes.Shape, i, j))
    //                 {
    //                     // Xử lý logic tiếp theo nếu tìm được khối phù hợp
    //                     // Thay vị trí block đó vào ma trận _tempMatrix
    //                     // Xét xem số hàng và cột ăn được là bao nhiêu sau đó thêm vào danh sách
    //                     _shapesFitList.Add(new InfoShape(shapes.Shape, shapes.Type, i, j));
    //                 }
    //             }
    //         }
    //     }
    //
    //     // Sau khi đã duyệt hết các khối bắt đầu xét khối tối ưu nhất trong danh sách các khối có thể vừa với bảng
    //     CheckInfoShapeList();
    // }
    //
    // private void CheckInfoShapeList()
    // {
    //     // CheckSpaceLeft();
    //     // Debug.LogError("ShapesFitList: " + _shapesFitList.Count);
    //     if (_shapesFitList.Count > 0)
    //     {
    //         List<int> tempSum = new List<int>();
    //         foreach (var items in _shapesFitList)
    //         {
    //             int Sum = 0;
    //             var shapeRows = items.Shape.GetLength(0);
    //             var shapeCols = items.Shape.GetLength(1);
    //             var rowPos = items.Row;
    //             var colPos = items.Col;
    //             var tempMatrix = _tempMatrix.Clone() as int[,];
    //
    //             // Thay khối vào ma trận tạm thời
    //             for (var row = 0; row < shapeRows; row++)
    //             {
    //                 for (var col = 0; col < shapeCols; col++)
    //                 {
    //                     tempMatrix[rowPos + row, colPos + col] = 1;
    //                 }
    //             }
    //
    //             // Kiểm tra xem hàng và cột nào đã đầy
    //             for (int row = 0; row < tempMatrix.GetLength(0); row++)
    //             {
    //                 if (IsRowFull(tempMatrix, row))
    //                 {
    //                     Sum++;
    //                 }
    //             }
    //
    //             for (int col = 0; col < tempMatrix.GetLength(1); col++)
    //             {
    //                 if (IsColumnFull(tempMatrix, col))
    //                 {
    //                     Sum++;
    //                 }
    //             }
    //
    //             // Debug.LogError("Sum: " + Sum);
    //
    //             // Xem khối nào sau khi thay vào thì ăn được nhiều nhất
    //             if (Sum > _bestSum)
    //             {
    //                 _bestSum = Sum;
    //                 tempSum.Clear();
    //                 tempSum.Add(items.Type);
    //             }
    //
    //             if (Sum == _bestSum)
    //             {
    //                 if (!tempSum.Contains(items.Type))
    //                 {
    //                     tempSum.Add(items.Type);
    //                 }
    //             }
    //         }
    //
    //         _typeSpawn = tempSum[Random.Range(0, tempSum.Count)];
    //
    //         if (_bestSum == 0)
    //         {
    //             var randomType = Random.Range(0, _shapesFitList.Count);
    //             _typeSpawn = _shapesFitList[randomType].Type;
    //         }
    //
    //         // Thêm vào danh sách spawn khối
    //         TypeSpawn.Add(_typeSpawn);
    //
    //         _row = _shapesFitList.Find(x => x.Type == _typeSpawn).Row;
    //         _col = _shapesFitList.Find(x => x.Type == _typeSpawn).Col;
    //
    //         // Sau khi biết được khối tối ưu nhất thì thay vào _tempMatrix để tính 2 khối tiếp theo
    //         // Debug.LogError("TypeSpawn: " + _typeSpawn);
    //         var Row = BlockShapes[_typeSpawn].Shape.GetLength(0);
    //         var Col = BlockShapes[_typeSpawn].Shape.GetLength(1);
    //
    //         List<int> fullRows = new List<int>();
    //         List<int> fullCols = new List<int>();
    //
    //         for (int row = 0; row < Row; row++)
    //         {
    //             for (int col = 0; col < Col; col++)
    //             {
    //                 _tempMatrix[_row + row, _col + col] = 1;
    //             }
    //         }
    //
    //         for (int row = 0; row < _tempMatrix.GetLength(0); row++)
    //         {
    //             if (IsRowFull(_tempMatrix, row))
    //             {
    //                 fullRows.Add(row);
    //             }
    //         }
    //
    //         for (int col = 0; col < _tempMatrix.GetLength(1); col++)
    //         {
    //             if (IsColumnFull(_tempMatrix, col))
    //             {
    //                 fullCols.Add(col);
    //             }
    //         }
    //
    //         ClearFullRowsAndColumns(_tempMatrix, fullRows, fullCols);
    //     }
    //     else
    //     {
    //         Debug.LogError("Random Shape");
    //         // Không còn khối nào phù hợp với bảng hiện tại -> Sinh ra ngẫu nhiên
    //         var randomType = Random.Range(0, BlockShapes.Length);
    //         TypeSpawn.Add(randomType);
    //     }
    // }

    private void ClearFullRowsAndColumns(int[,] matrix, List<int> fullRows, List<int> fullCols)
    {
        // Xóa hàng đầy
        foreach (int row in fullRows)
        {
            for (int col = 0; col < matrix.GetLength(1); col++)
            {
                matrix[row, col] = 0;
            }
        }

        // Xóa cột đầy
        foreach (int col in fullCols)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                matrix[row, col] = 0;
            }
        }
    }

    private bool IsRowFull(int[,] matrix, int row)
    {
        int cols = matrix.GetLength(1); // Số cột trong ma trận
        for (int col = 0; col < cols; col++)
        {
            if (matrix[row, col] != 1)
            {
                return false; // Nếu bất kỳ ô nào không phải 1, trả về false
            }
        }

        return true; // Toàn bộ dòng đều là 1
    }

    private bool IsColumnFull(int[,] matrix, int col)
    {
        int rows = matrix.GetLength(0); // Số hàng trong ma trận
        for (int row = 0; row < rows; row++)
        {
            if (matrix[row, col] != 1)
            {
                return false; // Nếu bất kỳ ô nào không phải 1, trả về false
            }
        }

        return true; // Toàn bộ cột đều là 1
    }

    // private bool DoesShapeFit(int[,] shape, int startRow, int startCol)
    // {
    //     int shapeRows = shape.GetLength(0);
    //     int shapeCols = shape.GetLength(1);
    //
    //     for (int row = 0; row < shapeRows; row++)
    //     {
    //         for (int col = 0; col < shapeCols; col++)
    //         {
    //             if (shape[row, col] != _tempMatrix[startRow + row, startCol + col])
    //             {
    //                 return false;
    //             }
    //         }
    //     }
    //
    //     return true;
    // }
    //
    // private void CheckSpaceLeft()
    // {
    //     _spaceLeft = false;
    //     int maxZeroCount = -1;
    //     for (int startRow = 0; startRow <= 8 - 5; startRow++)
    //     {
    //         for (int startCol = 0; startCol <= 8 - 5; startCol++)
    //         {
    //             int zeroCount = 0; // Đếm số lượng 0 trong ô 5x5 hiện tại
    //
    //             for (int row = 0; row < 5; row++)
    //             {
    //                 for (int col = 0; col < 5; col++)
    //                 {
    //                     if (_tempMatrix[startRow + row, startCol + col] == 0)
    //                     {
    //                         zeroCount++;
    //                     }
    //                 }
    //             }
    //
    //             if (zeroCount > maxZeroCount)
    //             {
    //                 maxZeroCount = zeroCount;
    //             }
    //         }
    //     }
    //
    //     if (maxZeroCount > 20)
    //     {
    //         _spaceLeft = true;
    //     }
    // }

    #endregion

    #region ============================= New Logic 2 =============================

    public class BestShape
    {
        public int Type { get; set; }
        public int StartRow { get; set; }
        public int StartCol { get; set; }
        public int Sum { get; set; }
    }

    private List<BestShape> _bestShapesList = new List<BestShape>();

    public void SwapBlock()
    {
        _shapeStorage.SwapShapes();
    }
    
    public void DetermineNewShapeTypes(int count)
    {
        TypeSpawn.Clear();
        // Thiết lập _tempMatrix từ trạng thái hiện tại
        for (int i = 0; i < CurrentState.Length; i++)
        {
            int row = i / 8;
            int col = i % 8;
            _tempMatrix[row, col] = CurrentState[i].SquareOccupied ? 1 : 0;
        }
        for (int i = 0; i < count; i++)
        {
            _bestShapesList.Clear();
            CheckNextShape2();
        }
    }
    
    public void PrepareNextSpawnShapes2()
    {
        TypeSpawn.Clear();
        GetCurrentBoard2();
        _shapeStorage.RequestNewShapes();
    }

    private void GetCurrentBoard2()
    {
        for (int i = 0; i < CurrentState.Length; i++)
        {
            int row = i / 8; // Lấy chỉ số hàng (dòng)
            int col = i % 8; // Lấy chỉ số cột

            _tempMatrix[row, col] = CurrentState[i].SquareOccupied ? 1 : 0; // 1: True, 0: False
        }

        for (int i = 0; i < 3; i++)
        {
            _bestShapesList.Clear();
            CheckNextShape2();
        }
    }

    private void CheckNextShape2()
    {
        foreach (var shapes in BlockShapes)
        {
            var shapeRows = shapes.Shape.GetLength(0);
            var shapeCols = shapes.Shape.GetLength(1);
            var shapeType = shapes.Type;

            for (int i = 0; i <= 8 - shapeRows; i++)
            {
                for (int j = 0; j <= 8 - shapeCols; j++)
                {
                    if (CheckFit(shapes.Shape, i, j))
                    {
                        // Xử lý logic tiếp theo nếu tìm được khối phù hợp
                        // Thay vị trí block đó vào ma trận _tempMatrix
                        // Xét xem số hàng và cột ăn được là bao nhiêu sau đó thêm vào danh sách
                        ApplyShape(shapeType, shapes.Shape, i, j);
                    }
                }
            }
        }

        // Check trong dic xem khối nào ăn được nhiều nhất
        CheckBestShape();
    }

    private void CheckBestShape()
    {
        if (_bestShapesList.Count == 0)
        {
            // Không có khối phù hợp, chọn ngẫu nhiên
            _typeSpawn = Random.Range(0, BlockShapes.Length);
            TypeSpawn.Add(_typeSpawn);
            return;
        }

        // Tìm giá trị Sum lớn nhất
        int maxSum = _bestShapesList.Max(shape => shape.Sum);

        // Lọc tất cả các khối có Sum bằng maxSum
        var bestCandidates = _bestShapesList
            .Where(shape => shape.Sum == maxSum)
            .ToList();

        // Chọn ngẫu nhiên 1 khối từ danh sách
        var selectedShape = bestCandidates[Random.Range(0, bestCandidates.Count)];

        // Thêm type vào danh sách spawn
        TypeSpawn.Add(selectedShape.Type);

        // Áp dụng khối này vào ma trận _tempMatrix và xử lý hàng/cột
        ApplyBestShapeToMatrix(selectedShape);
    }

    private void ApplyBestShapeToMatrix(BestShape shape)
    {
        // Lấy thông tin khối
        var blockShape = BlockShapes[shape.Type];
        int startRow = shape.StartRow;
        int startCol = shape.StartCol;

        // Áp dụng khối vào _tempMatrix
        for (int row = 0; row < blockShape.Shape.GetLength(0); row++)
        {
            for (int col = 0; col < blockShape.Shape.GetLength(1); col++)
            {
                _tempMatrix[startRow + row, startCol + col] = 1;
            }
        }

        // Kiểm tra hàng/cột đầy
        List<int> fullRows = new List<int>();
        List<int> fullCols = new List<int>();

        for (int row = 0; row < 8; row++)
        {
            if (IsRowFull(_tempMatrix, row)) fullRows.Add(row);
        }

        for (int col = 0; col < 8; col++)
        {
            if (IsColumnFull(_tempMatrix, col)) fullCols.Add(col);
        }

        // Xóa hàng/cột đầy
        ClearFullRowsAndColumns(_tempMatrix, fullRows, fullCols);
    }

    private bool CheckFit(int[,] shape, int startRow, int startCol)
    {
        int shapeRows = shape.GetLength(0);
        int shapeCols = shape.GetLength(1);

        for (int row = 0; row < shapeRows; row++)
        {
            for (int col = 0; col < shapeCols; col++)
            {
                if (shape[row, col] != _tempMatrix[startRow + row, startCol + col])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void ApplyShape(int shapeType, int[,] shape, int startRow, int startCol)
    {
        int shapeRows = shape.GetLength(0);
        int shapeCols = shape.GetLength(1);
        var tempMatrix = _tempMatrix.Clone() as int[,];

        // Áp dụng shape vào ma trận tạm thời
        for (int row = 0; row < shapeRows; row++)
        {
            for (int col = 0; col < shapeCols; col++)
            {
                tempMatrix[startRow + row, startCol + col] = 1;
            }
        }

        // Kiểm tra và xoá các hàng/cột đã đầy
        List<int> fullRows = new List<int>();
        List<int> fullCols = new List<int>();

        // Kiểm tra hàng đầy
        for (int row = 0; row < tempMatrix.GetLength(0); row++)
        {
            if (IsRowFull(tempMatrix, row))
            {
                fullRows.Add(row);
            }
        }

        // Kiểm tra cột đầy
        for (int col = 0; col < tempMatrix.GetLength(1); col++)
        {
            if (IsColumnFull(tempMatrix, col))
            {
                fullCols.Add(col);
            }
        }

        // Tính tổng số hàng/cột đã ăn
        int sum = fullRows.Count + fullCols.Count;

        // Thêm vào danh sách _sumList
        _bestShapesList.Add(new BestShape
        {
            Type = shapeType,
            StartRow = startRow,
            StartCol = startCol,
            Sum = sum
        });
    }

    #endregion
}