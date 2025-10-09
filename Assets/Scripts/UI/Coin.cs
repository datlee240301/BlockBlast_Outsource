using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;
    
    private void OnEnable()
    {
        _coinText.text = ResourceManager.Coins.ToString();
        GameEventManager.CoinCollected += UpdateCoin;
    }
    
    private void OnDisable()
    {
        GameEventManager.CoinCollected -= UpdateCoin;
    }
    
    private void UpdateCoin(int coin)
    {
        ResourceManager.Coins += coin;
        _coinText.text = ResourceManager.Coins.ToString();
    }
}
