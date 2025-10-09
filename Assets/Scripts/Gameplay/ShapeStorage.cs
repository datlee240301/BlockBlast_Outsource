using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeStorage : MonoBehaviour
{
    [SerializeField] private List<ShapeData> _shapeDataList;
    public List<Shape> ShapeList;

    private void Start()
    {
        foreach (var shape in ShapeList)
        {
            var shapeIndex = Random.Range(0, _shapeDataList.Count);
            shape.CreateShape(_shapeDataList[shapeIndex]);
        }
    }

    public void RequestNewShapes()
    {
        ClearListBeforeRequest();

        for (int i = 0; i < ShapeList.Count; i++)
        {
            var num = GameController.Instance.TypeSpawn[i];
            ShapeList[i].RequestNewShape(_shapeDataList[num]);
        }
    }
    
    public void SwapShapes()
    {
        List<Shape> shapesToSwap = new List<Shape>();
        for (int i = 0; i < ShapeList.Count; i++)
        {
            if (ShapeList[i].IsOnStartPosition() && ShapeList[i].IsAnyOfShapeSquareActive())
            {
                shapesToSwap.Add(ShapeList[i]);
            }
        }
        int numToSwap = shapesToSwap.Count;
        if (numToSwap == 0) return;

        GameController.Instance.DetermineNewShapeTypes(numToSwap);

        for (int j = 0; j < numToSwap; j++)
        {
            int type = GameController.Instance.TypeSpawn[j];
            shapesToSwap[j].RequestNewShape(_shapeDataList[type]);
        }
    }
    
    private void ClearListBeforeRequest()
    {
        GameController.Instance.WeaponIndexList.Clear();
        GameController.Instance.SpecialIndexList.Clear();
    }

    public Shape GetCurrentSelectedShape()
    {
        foreach (var shape in ShapeList)
        {
            if (!shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
            {
                return shape;
            }
        }

        return null;
    }
}