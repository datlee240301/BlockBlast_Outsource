using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RevivePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private int _reviveTime = 9;
    private readonly WaitForSeconds _time1s = new WaitForSeconds(1);
    
    private void OnEnable()
    {
        AudioManager.StopMusic();
        AudioManager.PlaySound("Revive");
        MainUIManager.Instance.PopupOpened = true;
        StartCoroutine(StartTimer());
    }

    private void OnDisable(){
        MainUIManager.Instance.PopupOpened = false;
    }
    
    private IEnumerator StartTimer()
    {
        int timer = _reviveTime;
        while (timer > 0)
        {
            _timerText.text = timer.ToString();
            yield return _time1s;
            timer--;
        }
        if (timer == 0)
        {
            gameObject.SetActive(false);
            GameUIManager.Instance.ShowEndPanel();
        }
    }
    
    public void OnClickReviveCoins()
    {
        AudioManager.PlaySound("Click");
        if (ResourceManager.Coins >= 15)
        {
            GameEventManager.CoinCollected?.Invoke(-15);
            GameUIManager.Instance.SetSortingOrder(1);
            GameController.Instance.PrepareNextSpawnShapes2();
            gameObject.SetActive(false);
            AudioManager.PlayLoopSound("MainTheme");
        }
        else
        {
            GameUIManager.Instance.ShowNotEnoughCoinsPopup();
        }
    }
}
