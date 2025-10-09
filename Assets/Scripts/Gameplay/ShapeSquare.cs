using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShapeSquare : MonoBehaviour
{
    public enum Type
    {
        Weapon,
        Special,
        None
    }

    public Type SquareType;
    public int ID;
    public int Color;
    [SerializeField] private Image _occupiedImage;
    [SerializeField] private Image _itemImage;
    [SerializeField] private List<Sprite> _itemSprites;
    [SerializeField] private List<Sprite> _colorBlockSprites;
    [SerializeField] private Image _selfImage;

    private void Start()
    {
        // _occupiedImage.gameObject.SetActive(false);
        // _itemImage.gameObject.SetActive(false);
    }

    // public void DeactivateShape()
    // {
    //     gameObject.GetComponent<BoxCollider2D>().enabled = false;
    //     gameObject.SetActive(false);
    // }
    
    public void SetTypeColor(int index)
    {
        Color = index;
        _selfImage.sprite = _colorBlockSprites[index];
    }

    public void ActivateShape()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    // public void SetOccupied()
    // {
    //     _occupiedImage.gameObject.SetActive(true);
    // }

    public void SetItemOnSquare()
    {
        switch (SquareType)
        {
            case Type.Weapon:
                int rand = Random.Range(0, 10);

                if (rand <= 4)
                {
                    SetItem(0);
                }
                else if (rand <= 7)
                {
                    SetItem(1);
                }
                else
                {
                    SetItem(2);
                }
                break;
            case Type.Special:
                int rand2 = Random.Range(0, 10);
                SetItem(rand2 < 4 ? 3 : 4);
                break;
            case Type.None:
                _itemImage.gameObject.SetActive(false);
                break;
        }
    }
    
    public int GetIconIndex()
    {
        if (_itemImage.sprite == null) return -1;
        return _itemSprites.IndexOf(_itemImage.sprite);
    }

    private void SetItem(int index)
    {
        _itemImage.gameObject.SetActive(true);
        _itemImage.sprite = _itemSprites[index];
    }

    // public void UnsetOccupied()
    // {
    //     _occupiedImage.gameObject.SetActive(false);
    // }

    #region ========== LOGIC HOOVER SHAPE ==========
    
    private float _lastTriggerTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GridSquare gridSquare = other.GetComponent<GridSquare>();
        if (gridSquare != null)
        {
            GetComponentInParent<Shape>()?.AddOverlappedGridSquare(gridSquare);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time - _lastTriggerTime < 0.05f) return;
        _lastTriggerTime = Time.time;

        GridSquare gridSquare = other.GetComponent<GridSquare>();
        if (gridSquare != null)
        {
            GetComponentInParent<Shape>()?.AddOverlappedGridSquare(gridSquare);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GridSquare gridSquare = other.GetComponent<GridSquare>();
        if (gridSquare != null)
        {
            GetComponentInParent<Shape>()?.RemoveOverlappedGridSquare(gridSquare);
        }
    }

    #endregion
}