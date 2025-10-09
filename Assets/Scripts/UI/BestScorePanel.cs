using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestScorePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _highScoreText;
    
    private void OnEnable()
    {
        AudioManager.StopMusic();
        MainUIManager.Instance.PopupOpened = true;
        _highScoreText.text = ResourceManager.HighScore.ToString();
    }

    private void OnDisable(){
        MainUIManager.Instance.PopupOpened = false;
    }
    
    public void OnClickHome()
    {
        AudioManager.PlaySound("Click");
        GameEventManager.ReturnPoolObjects?.Invoke();
        MainUIManager.LoadScene("HomeScreen");
    }
}
