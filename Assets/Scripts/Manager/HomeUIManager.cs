using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HomeUIManager : Singleton<HomeUIManager>
{
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private SettingPanel _settingPanel;
    [SerializeField] private ThemePanel _themePanel;
    [SerializeField] private ShopPanel _shopPanel;
    private bool _isClicked;
    
    private void OnEnable()
    {
        Setup();
        _highScoreText.text = ResourceManager.HighScore.ToString();
    }

    private void Setup()
    {
        AudioManager.PlayLoopSound("MainTheme");
        _settingPanel.gameObject.SetActive(false);
        _themePanel.gameObject.SetActive(false);
        _shopPanel.gameObject.SetActive(false);
    }
    
    public void OnClickShop()
    {
        AudioManager.PlaySound("Click");
        _shopPanel.gameObject.SetActive(true);
    }
    
    public void ShowShop()
    {
        _shopPanel.gameObject.SetActive(true);
    }
    
    public void OnClickTheme()
    {
        AudioManager.PlaySound("Click");
        _themePanel.gameObject.SetActive(true);
    }
    
    public void OnClickSetting()
    {
        AudioManager.PlaySound("Click");
        _settingPanel.gameObject.SetActive(true);
    }
    
    public void StartGame()
    {
        if (_isClicked) return;
        _isClicked = true;
        AudioManager.PlaySound("Click");
        MainUIManager.LoadScene("Gameplay");
    }
}
