using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemePanel : MonoBehaviour
{
    [SerializeField] private Image[] _themeBg;
    [SerializeField] private GameObject[] _checkMarks;
    [SerializeField] private GameObject[] _masks;
    [SerializeField] private TextMeshProUGUI _themeCount;
    [SerializeField] private Sprite _themeUnequipped;
    [SerializeField] private Sprite _themeEquipped;

    private bool[] _isUnlocked;

    private void OnEnable()
    {
        Setup();
    }

    private void Setup()
    {
        _isUnlocked = new bool[]
        {
            ResourceManager.Theme1, ResourceManager.Theme2, ResourceManager.Theme3,
            ResourceManager.Theme4, ResourceManager.Theme5, ResourceManager.Theme6,
            ResourceManager.Theme7, ResourceManager.Theme8, ResourceManager.Theme9,
        };

        for (int i = 0; i < _checkMarks.Length; i++)
        {
            _masks[i].SetActive(!_isUnlocked[i]);
            _checkMarks[i].SetActive(false);
            _themeBg[i].sprite = _themeUnequipped;
        }

        _checkMarks[ResourceManager.ThemeEquipped].SetActive(true);
        _themeCount.text = ResourceManager.Theme.ToString();
        _themeBg[ResourceManager.ThemeEquipped].sprite = _themeEquipped;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnClickSelectTheme(int themeIndex)
    {
        AudioManager.PlaySound("Click");

        if (_isUnlocked[themeIndex])
        {
            if (ResourceManager.ThemeEquipped == themeIndex) return;

            ResourceManager.ThemeEquipped = themeIndex;
            Setup();
            return;
        }

        if (ResourceManager.Theme > 0)
        {
            ResourceManager.Theme--;
            SetThemeUnlocked(themeIndex);
            ResourceManager.ThemeEquipped = themeIndex;
            Setup();
        }
        else
        {
            HomeUIManager.Instance.ShowShop();
        }
    }
    
    private void SetThemeUnlocked(int index)
    {
        switch (index)
        {
            case 0: ResourceManager.Theme1 = true; break;
            case 1: ResourceManager.Theme2 = true; break;
            case 2: ResourceManager.Theme3 = true; break;
            case 3: ResourceManager.Theme4 = true; break;
            case 4: ResourceManager.Theme5 = true; break;
            case 5: ResourceManager.Theme6 = true; break;
            case 6: ResourceManager.Theme7 = true; break;
            case 7: ResourceManager.Theme8 = true; break;
            case 8: ResourceManager.Theme9 = true; break;
        }
    }

}