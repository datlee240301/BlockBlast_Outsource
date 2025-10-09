using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMoreSwapPanel : MonoBehaviour
{
    private void OnEnable()
    {
        MainUIManager.Instance.PopupOpened = true;
        Time.timeScale = 0;
    }
    
    public void Close()
    {
        AudioManager.PlaySound("Click");
        MainUIManager.Instance.PopupOpened = false;
        Time.timeScale = 1;
        GameUIManager.Instance.SetSortingOrder(1);
        gameObject.SetActive(false);
    }
    
    public void OnClickGetMoreSwap()
    {
        AudioManager.PlaySound("Click");
        if (ResourceManager.Coins > 50)
        {
            GameEventManager.CoinCollected?.Invoke(-50);
            GameEventManager.SwapCollected?.Invoke(2);
            MainUIManager.Instance.PopupOpened = false;
            Time.timeScale = 1;
            GameUIManager.Instance.SetSortingOrder(1);
            gameObject.SetActive(false);
        }
        else
        {
            GameUIManager.Instance.ShowShop();
        }
    }
}
