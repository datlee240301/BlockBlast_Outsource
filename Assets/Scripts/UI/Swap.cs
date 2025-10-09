using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Swap : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _swapText;
    [SerializeField] private GameObject _plusSwap;
    
    private void OnEnable()
    {
        _swapText.text = ResourceManager.Swap.ToString();
        _plusSwap.SetActive(ResourceManager.Swap <= 0);
        GameEventManager.SwapCollected += UpdateSwap;
    }
    
    private void OnDisable()
    {
        GameEventManager.SwapCollected -= UpdateSwap;
    }
    
    private void UpdateSwap(int swap)
    {
        ResourceManager.Swap += swap;
        _swapText.text = ResourceManager.Swap.ToString();
        _plusSwap.SetActive(ResourceManager.Swap <= 0);
    }

    public void OnClickSwap()
    {
        AudioManager.PlaySound("Click");
        if (ResourceManager.Swap > 0)
        {
            GameEventManager.SwapCollected?.Invoke(-1);
            GameController.Instance.SwapBlock();
        }
        else
        {
            GameUIManager.Instance.ShowGetMoreSwapPanel();
        }
    }
}
