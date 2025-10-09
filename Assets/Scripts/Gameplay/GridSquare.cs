using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public enum Type
    {
        Weapon,
        Special,
        None
    }
    
    public Type SquareType;
    public int ItemType;
    [SerializeField] private Image _image;
    [SerializeField] private Image _hooverImage;
    [SerializeField] private Image _activeImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private List<Sprite> _iconSprites;
    [SerializeField] private List<Sprite> _colorBlockSprites;
    
    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }

    private void Start()
    {
        ClearOccupied();
    }

    public void ReturnPool()
    {
        ClearOccupied();
        Deactivate();
        SquareType = Type.None;
        ItemType = 0;
        transform.position = ObjectPooler.Instance.Root.position;
        transform.SetParent(ObjectPooler.Instance.Root);
        gameObject.SetActive(false);
    }

    public void PlaceShapeOnBoard(bool haveIcon, int iconIndex, int color)
    {
        ActivateSquare(haveIcon, iconIndex, color);
    }
    
    // public bool CanUseThisSquare()
    // {
    //     return _hooverImage.gameObject.activeSelf;
    // }
    
    private void ActivateSquare(bool haveIcon, int iconIndex, int color)
    {
        _hooverImage.gameObject.SetActive(false);
        _activeImage.gameObject.SetActive(true);
        _activeImage.sprite = _colorBlockSprites[color];
        if (haveIcon && iconIndex >= 0 && iconIndex < _iconSprites.Count)
        {
            _iconImage.gameObject.SetActive(true);
            _iconImage.sprite = _iconSprites[iconIndex];
            ItemType = iconIndex;
        }
        else
        {
            _iconImage.gameObject.SetActive(false);
        }
        Selected = true;
        SquareOccupied = true;
    }

    public void Deactivate()
    {
        _activeImage.gameObject.SetActive(false);
        _iconImage.gameObject.SetActive(false);
    }
    
    public void ClearOccupied()
    {
        Selected = false;
        SquareOccupied = false;
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (!SquareOccupied)
    //     {
    //         Selected = true;
    //         _hooverImage.gameObject.SetActive(true);
    //     }
    //     // else if (other.GetComponent<ShapeSquare>() != null)
    //     // {
    //     //     other.GetComponent<ShapeSquare>().SetOccupied();
    //     // }
    // }
    //
    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     Selected = true;
    //     if (!SquareOccupied)
    //     {
    //         _hooverImage.gameObject.SetActive(true);
    //     }
    //     // else if (other.GetComponent<ShapeSquare>() != null)
    //     // {
    //     //     other.GetComponent<ShapeSquare>().SetOccupied();
    //     // }
    // }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!SquareOccupied)
        {
            Selected = false;
            _hooverImage.gameObject.SetActive(false);
        }
        // else if (other.GetComponent<ShapeSquare>() != null)
        // {
        //     other.GetComponent<ShapeSquare>().UnsetOccupied();
        // }
    }
    
    public void SetHoover(bool show)
    {
        if (!SquareOccupied)
        {
            _hooverImage.gameObject.SetActive(show);
            Selected = show;
        }
        else
        {
            _hooverImage.gameObject.SetActive(false);
            Selected = false;
        }
    }
}
