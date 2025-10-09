using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText, _highScoreText;
    
    private void OnEnable()
    {
        AudioManager.StopMusic();
        AudioManager.PlaySound("LevelComplete");
        MainUIManager.Instance.PopupOpened = true;
        _scoreText.text = ResourceManager.Score.ToString();
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
