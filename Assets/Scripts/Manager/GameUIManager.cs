using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager>
{
    [Header("==================== THEMES ====================")] 
    [SerializeField] private Image _bg;
    [SerializeField] private List<Sprite> _bgSprites;
    [Header("==================== OTHER ====================")]
    public Transform Target;
    [SerializeField] private GameOverPanel _gameOverPanel;
    [SerializeField] private BestScorePanel _bestScorePanel;
    [SerializeField] private RevivePanel _revivePanel;
    [SerializeField] private NotEnoughCoinPopup _notEnoughCoinsPopup;
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private ShopPanel _shopPanel;
    [SerializeField] private GetMoreSwapPanel _getMoreSwapPanel;
    [SerializeField] private HealthbarController _healthbar;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    private Canvas _canvas;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        Setup();
        ObjectPooler.Instance.MoveToPool();
    }
    
    public void UpdateHealth(int amount)
    {
        _healthbar.UpdateHealth(amount);
    }
    
    private void Setup()
    {
        _gameOverPanel.gameObject.SetActive(false);
        _bestScorePanel.gameObject.SetActive(false);
        _notEnoughCoinsPopup.gameObject.SetActive(false);
        _revivePanel.gameObject.SetActive(false);
        _pausePanel.gameObject.SetActive(false);
        _shopPanel.gameObject.SetActive(false);
        _getMoreSwapPanel.gameObject.SetActive(false);
        MainUIManager.Instance.PopupOpened = false;
        
        ResourceManager.Score = 0;
        _scoreText.text = ResourceManager.Score.ToString();
        _highScoreText.text = ResourceManager.HighScore.ToString();
        _bg.sprite = _bgSprites[ResourceManager.ThemeEquipped];
    }
    
    public void UpdateScore()
    {
        if (ResourceManager.Score > ResourceManager.HighScore)
        {
            ResourceManager.HighScore = ResourceManager.Score;
            _highScoreText.text = ResourceManager.Score.ToString();
        }
        _scoreText.text = ResourceManager.Score.ToString();
    }
    
    public void ShowEndPanel()
    {
        SetSortingOrder(5);
        if (ResourceManager.Score >= ResourceManager.HighScore)
        {
            ResourceManager.HighScore = ResourceManager.Score;
            _bestScorePanel.gameObject.SetActive(true);
        }
        else
        {
            _gameOverPanel.gameObject.SetActive(true);
        }
    }
    
    public void ShowGetMoreSwapPanel()
    {
        SetSortingOrder(6);
        _getMoreSwapPanel.gameObject.SetActive(true);
    }
    
    public void ShowRevivePanel()
    {
        SetSortingOrder(5);
        _revivePanel.gameObject.SetActive(true);
    }
    
    public void ShowNotEnoughCoinsPopup()
    {
        SetSortingOrder(6);
        _notEnoughCoinsPopup.gameObject.SetActive(true);
    }

    public void ShowShop()
    {
        SetSortingOrder(7);
        _shopPanel.gameObject.SetActive(true);
    }
    
    public void SetSortingOrder(int order)
    {
        _canvas.sortingOrder = order;
    }
    
    public void OnClickHome()
    {
        AudioManager.PlaySound("Click");
        SetSortingOrder(5);
        _pausePanel.gameObject.SetActive(true);
    }
}
