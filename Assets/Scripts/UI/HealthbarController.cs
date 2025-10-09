using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    [SerializeField] private Slider _healthbar;
    [SerializeField] private Image _fill;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private float _fullHealth;
    [SerializeField] private Image _valueChangeImage;
    [SerializeField] private Sprite _positiveValueSprite, _negativeValueSprite;
    [SerializeField] private Color _fullHealthColor, _halfHealthColor, _lowHealthColor;
    private readonly WaitForSeconds _timeOff = new WaitForSeconds(0.5f);
    
    private void Start()
    {
        _healthbar.maxValue = _fullHealth;
        _healthbar.value = _fullHealth;
        UpdateText();
        UpdateColor();
    }

    public void UpdateHealth(int health)
    {
        _healthbar.value += health;
        UpdateText();
        if (_healthbar.value > _fullHealth)
        {
            _healthbar.value = _fullHealth;
        }
        else if (_healthbar.value <= 0)
        {
            GameUIManager.Instance.ShowEndPanel();
            _healthbar.value = 0;
        }
        StartCoroutine(ChangeValueImage(health));
        UpdateColor();
    }
    
    private void UpdateText()
    {
        _healthText.text = _healthbar.value.ToString() + "%";
    }
    
    private IEnumerator ChangeValueImage(int health)
    {
        _valueChangeImage.gameObject.SetActive(true);
        _valueChangeImage.sprite = health > 0 ? _positiveValueSprite : _negativeValueSprite;
        yield return _timeOff;
        _valueChangeImage.gameObject.SetActive(false);
    }
    
    private void UpdateColor()
    {
        if (_healthbar.value > _fullHealth / 2)
        {
            _fill.color = _fullHealthColor;
        }
        else if (_healthbar.value > _fullHealth / 4)
        {
            _fill.color = _halfHealthColor;
        }
        else
        {
            _fill.color = _lowHealthColor;
        }
    }
}
