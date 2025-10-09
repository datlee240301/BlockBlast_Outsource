using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Shape : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public ShapeData CurrentShapeData;
    public int TotalSquareNumber;
    [SerializeField] private GameObject _squareShapeImage;
    [SerializeField] private Vector3 _shapeSelectedScale;
    [SerializeField] private Vector2 _offset = new Vector2(0, 700f);
    private List<GameObject> _currentShape = new();
    private Vector3 _shapeOriginalScale;
    private RectTransform _transform;
    private Canvas _canvas;
    private Vector3 _startPosition;
    private bool _shapeActived;

    private void Awake()
    {
        _shapeOriginalScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _shapeActived = true;
        _startPosition = _transform.localPosition;
    }

    private void OnEnable()
    {
        GameEventManager.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEventManager.SetShapeInactive += SetShapeInactive;
    }

    private void OnDisable()
    {
        GameEventManager.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEventManager.SetShapeInactive -= SetShapeInactive;
    }

    private float GetPositionForSquare(int size, int index, Vector2 moveDistance, bool isHorizontal)
    {
        if (size <= 1) return 0;

        float middleIndex = (size - 1) / 2f;
        float shift = (index - middleIndex) * (isHorizontal ? moveDistance.x : moveDistance.y);

        return shift;
    }

    private int GetNumberOfSquares(ShapeData shapeData)
    {
        return shapeData.Board.Sum(rowData => rowData.Column.Count(active => active));
    }

    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        TotalSquareNumber = GetNumberOfSquares(shapeData);
        var num = Random.Range(0, 5);

        while (_currentShape.Count < TotalSquareNumber)
        {
            var newSquare = Instantiate(_squareShapeImage, transform);
            _currentShape.Add(newSquare);
        }

        foreach (var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = _squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(
            squareRect.rect.width * squareRect.localScale.x,
            -squareRect.rect.height * squareRect.localScale.y // Đã thêm dấu âm
        );

        int currentIndex = 0;
        List<int> availableIndexes = new List<int>();

        // Lấy danh sách các ô active
        for (var row = 0; row < shapeData.Rows; row++)
        {
            for (var column = 0; column < shapeData.Columns; column++)
            {
                if (shapeData.Board[row].Column[column])
                {
                    availableIndexes.Add(currentIndex);
                    currentIndex++;
                }
            }
        }

        // Chọn ngẫu nhiên index cho Weapon và Special
        int weaponIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
        availableIndexes.Remove(weaponIndex);
        int specialIndex = TotalSquareNumber > 1 ? availableIndexes[Random.Range(0, availableIndexes.Count)] : -1;
        if (specialIndex != -1) availableIndexes.Remove(specialIndex);

        // Tạo danh sách sắp xếp theo thứ tự từ trên xuống, trái sang phải
        var sortedSquares = new List<(int row, int col, GameObject square)>();
        currentIndex = 0;
        for (var row = 0; row < shapeData.Rows; row++)
        {
            for (var column = 0; column < shapeData.Columns; column++)
            {
                if (shapeData.Board[row].Column[column])
                {
                    sortedSquares.Add((row, column, _currentShape[currentIndex]));
                    currentIndex++;
                }
            }
        }

        // Sắp xếp lại theo hàng TĂNG DẦN (từ trên xuống) và cột TĂNG DẦN (trái sang phải)
        sortedSquares.Sort((a, b) =>
        {
            if (a.row != b.row) return a.row.CompareTo(b.row);
            return a.col.CompareTo(b.col);
        });

        // Gán vị trí và Type
        currentIndex = 0;
        foreach (var (row, column, square) in sortedSquares)
        {
            square.SetActive(true);
            float x = GetPositionForSquare(shapeData.Columns, column, moveDistance, true);
            float y = GetPositionForSquare(shapeData.Rows, row, moveDistance, false);
            square.GetComponent<RectTransform>().localPosition = new Vector2(x, y);

            var squareScript = square.GetComponent<ShapeSquare>();
            if (currentIndex == weaponIndex)
            {
                squareScript.SquareType = ShapeSquare.Type.Weapon;
                GameController.Instance.WeaponIndexList.Add(currentIndex);
            }
            else if (currentIndex == specialIndex)
            {
                squareScript.SquareType = ShapeSquare.Type.Special;
                GameController.Instance.SpecialIndexList.Add(currentIndex);
            }
            else
            {
                squareScript.SquareType = ShapeSquare.Type.None;
            }

            squareScript.SetItemOnSquare();
            squareScript.SetTypeColor(num);
            squareScript.ID = currentIndex;
            currentIndex++;
        }

        // Sắp xếp lại Hierarchy theo ID
        _currentShape = _currentShape.OrderBy(square => square.GetComponent<ShapeSquare>().ID).ToList();
        for (int i = 0; i < _currentShape.Count; i++)
        {
            _currentShape[i].transform.SetSiblingIndex(i);
        }
    }

    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }

    public bool IsAnyOfShapeSquareActive()
    {
        return _currentShape.Any(square => square.activeSelf);
    }

    private void MoveShapeToStartPosition()
    {
        _transform.anchorMin = new Vector2(0.5f, 0.5f);
        _transform.anchorMax = new Vector2(0.5f, 0.5f);
        _transform.pivot = new Vector2(0.5f, 0.5f);

        _transform.transform.localPosition = _startPosition;
    }

    // public void DeactivateShape()
    // {
    //     if (_shapeActived)
    //     {
    //         foreach (var square in _currentShape)
    //         {
    //             square?.GetComponent<ShapeSquare>().DeactivateShape();
    //         }
    //     }
    //     _shapeActived = false;
    // }

    public void SetShapeInactive()
    {
        if (!IsOnStartPosition() && IsAnyOfShapeSquareActive())
        {
            foreach (var square in _currentShape)
            {
                square.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateShape()
    {
        if (!_shapeActived)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().ActivateShape();
            }
        }

        _shapeActived = true;
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        MoveShapeToStartPosition();
        CreateShape(shapeData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _shapeSelectedScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position,
            Camera.main, out pos);
        _transform.localPosition = pos + _offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _shapeOriginalScale;
        GameEventManager.CheckShapeDown?.Invoke();
    }

    #region ============ LOGIC HOOVER SHAPE ============
    
    private List<GridSquare> _overlappedGridSquares = new List<GridSquare>();
    
    public void AddOverlappedGridSquare(GridSquare gridSquare)
    {
        if (!_overlappedGridSquares.Contains(gridSquare))
            _overlappedGridSquares.Add(gridSquare);
    
        UpdateHooverState();
    }

    // Xóa GridSquare khỏi danh sách khi trigger exit
    public void RemoveOverlappedGridSquare(GridSquare gridSquare)
    {
        if (_overlappedGridSquares.Contains(gridSquare))
            _overlappedGridSquares.Remove(gridSquare);
    
        UpdateHooverState();
    }

    // Kiểm tra xem có ô nào bị chồng lên ô đã occupied không
    private void UpdateHooverState()
    {
        bool canShowHoover = _overlappedGridSquares.All(gs => !gs.SquareOccupied);
    
        foreach (var gridSquare in _overlappedGridSquares)
        {
            gridSquare.SetHoover(canShowHoover);
        }
    }
    
    #endregion
}