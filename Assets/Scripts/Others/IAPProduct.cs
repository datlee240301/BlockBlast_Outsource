using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class MyPurchaseID
{
    public const string StarterBundles = "com.blockblast.starterbundle";
    public const string ExtraBundles = "com.blockblast.extrabundle";
    public const string CoinPack1 = "com.blockblast.pack1";
    public const string CoinPack2 = "com.blockblast.pack2";
    public const string CoinPack3 = "com.blockblast.pack3";
    public const string CoinPack4 = "com.blockblast.pack4";
    public const string CoinPack5 = "com.blockblast.pack5";
    public const string CoinPack6 = "com.blockblast.pack6";
    public const string CoinPack7 = "com.blockblast.pack7";
    public const string CoinPack8 = "com.blockblast.pack8";
    public const string CoinPack9 = "com.blockblast.pack9";
}

public class IAPProduct : MonoBehaviour
{
    [SerializeField] private string _purchaseID;
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _discount;
    [SerializeField] private Sprite _icon;

    public string PurchaseID => _purchaseID;

    public delegate void PurchaseEvent(Product Model, Action OnComplete);

    public event PurchaseEvent OnPurchase;
    private Product _model;

    private void Start()
    {
        RegisterPurchase();
        RegisterEventButton();
    }

    protected virtual void RegisterPurchase()
    {
        StartCoroutine(IAPManager.Instance.CreateHandleProduct(this));
    }

    public void Setup(Product product, string code, string price)
    {
        _model = product;
        if (_price != null)
        {
            _price.text = price + " " + code;
        }

        if (_discount != null)
        {
            if (code.Equals("VND"))
            {
                var round = Mathf.Round(float.Parse(price) + float.Parse(price) * .4f);
                _discount.text = code + " " + round;
            }
            else
            {
                var priceFormat = $"{float.Parse(price) + float.Parse(price) * .4f:0.00}";
                _discount.text = code + " " + priceFormat;
            }
        }
    }

    private void RegisterEventButton()
    {
        _purchaseButton.onClick.AddListener(() =>
        {
            AudioManager.PlaySound("Click");
            Purchase();
        });
    }

    private void Purchase()
    {
        OnPurchase?.Invoke(_model, HandlePurchaseComplete);
    }

    private void HandlePurchaseComplete()
    {
        switch (_purchaseID)
        {
            case MyPurchaseID.StarterBundles:
                AddResourcePack(5,1,200);
                break;
            case MyPurchaseID.ExtraBundles:
                AddResourcePack(10,2,500);
                break;
            case MyPurchaseID.CoinPack1:
                AddResourcePack(0,0,100);
                break;
            case MyPurchaseID.CoinPack2:
                AddResourcePack(0,0,300);
                break;
            case MyPurchaseID.CoinPack3:
                AddResourcePack(0,0,500);
                break;
            case MyPurchaseID.CoinPack4:
                AddResourcePack(0,0,800);
                break;
            case MyPurchaseID.CoinPack5:
                AddResourcePack(0,0,1000);
                break;
            case MyPurchaseID.CoinPack6:
                AddResourcePack(0,0,1200);
                break;
            case MyPurchaseID.CoinPack7:
                AddResourcePack(0,0,1500);
                break;
            case MyPurchaseID.CoinPack8:
                AddResourcePack(0,0,1700);
                break;
            case MyPurchaseID.CoinPack9:
                AddResourcePack(0,0,2000);
                break;
        }

        if (_icon != null)
        {
            _purchaseButton.gameObject.GetComponent<Image>().sprite = _icon;
            _purchaseButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            _purchaseButton.interactable = false;
        }
    }
    
    private void AddResourcePack(int swaps, int themes, int coins)
    {
        ResourceManager.Swap += swaps;
        ResourceManager.Coins += coins;
        ResourceManager.Theme += themes;
        GameEventManager.UpdateShopResource?.Invoke();
    }
}