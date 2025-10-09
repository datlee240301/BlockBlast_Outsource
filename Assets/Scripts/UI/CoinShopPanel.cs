using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinShopPanel : MonoBehaviour
{
    private void OnEnable()
    {
        MainUIManager.Instance.PopupOpened = true;
    }

    private void OnDisable(){
        MainUIManager.Instance.PopupOpened = false;
    }

    public void Close()
    {
        AudioManager.PlaySound("Click");
        MainUIManager.Instance.PopupOpened = false;
        gameObject.SetActive(false);
    }
}
