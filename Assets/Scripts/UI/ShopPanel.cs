using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _swapText;
    [SerializeField] private TextMeshProUGUI _themeText;
    
    private void OnEnable()
    {
        UpdateUI();
        GameEventManager.UpdateShopResource += UpdateUI;
    }
    
    private void OnDisable()
    {
        GameEventManager.UpdateShopResource -= UpdateUI;
    }

    public void Back()
    {
        AudioManager.PlaySound("Click");
        gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        _coinText.text = ResourceManager.Coins.ToString();
        _swapText.text = ResourceManager.Swap.ToString();
        _themeText.text = ResourceManager.Theme.ToString();
    }
}
