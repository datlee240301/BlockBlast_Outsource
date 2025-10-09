using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private void OnEnable(){
        MainUIManager.Instance.PopupOpened = true;
        Time.timeScale = 0;
    }

    private void OnDisable(){
         MainUIManager.Instance.PopupOpened = false;
    }

    public void OnClickResume()
    {
        AudioManager.PlaySound("Click");
        Time.timeScale = 1;
        GameUIManager.Instance.SetSortingOrder(1);
        gameObject.SetActive(false);
    }
    
    public void OnClickHome()
    {
        AudioManager.PlaySound("Click");
        Time.timeScale = 1;
        GameEventManager.ReturnPoolObjects?.Invoke();
        MainUIManager.LoadScene("HomeScreen");
    }
}
