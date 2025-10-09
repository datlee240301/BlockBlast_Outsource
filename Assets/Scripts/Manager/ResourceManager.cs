using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public static bool RemoveAds
    {
        get => PlayerPrefs.GetInt("RemoveAds", 0) == 1;
        set => PlayerPrefs.SetInt("RemoveAds", value ? 1 : 0);
    }
    
    public static bool FirstOpen
    {
        get => PlayerPrefs.GetInt("FirstOpen", 0) == 1;
        set => PlayerPrefs.SetInt("FirstOpen", value ? 1 : 0);
    }
    
    public static int Score
    {
        get => PlayerPrefs.GetInt("Score", 0);
        set => PlayerPrefs.SetInt("Score", value);
    }
    
    public static int HighScore
    {
        get => PlayerPrefs.GetInt("HighScore", 0);
        set => PlayerPrefs.SetInt("HighScore", value);
    }
    
    public static int Health
    {
        get => PlayerPrefs.GetInt("Health", 5);
        set => PlayerPrefs.SetInt("Health", value);
    }
    
    public static int Coins
    {
        get => PlayerPrefs.GetInt("Coin", 10);
        set => PlayerPrefs.SetInt("Coin", value);
    }
    
    public static int Theme
    {
        get => PlayerPrefs.GetInt("Theme", 0);
        set => PlayerPrefs.SetInt("Theme", value);
    }
    
    public static int Swap
    {
        get => PlayerPrefs.GetInt("Swap", 0);
        set => PlayerPrefs.SetInt("Swap", value);
    }
    
    public static bool Theme1
    {
        get => PlayerPrefs.GetInt("Theme1", 1) == 1;
        set => PlayerPrefs.SetInt("Theme1", value ? 1 : 0);
    }
    
    public static bool Theme2
    {
        get => PlayerPrefs.GetInt("Theme2", 0) == 1;
        set => PlayerPrefs.SetInt("Theme2", value ? 1 : 0);
    }
    
    public static bool Theme3
    {
        get => PlayerPrefs.GetInt("Theme3", 0) == 1;
        set => PlayerPrefs.SetInt("Theme3", value ? 1 : 0);
    }
    
    public static bool Theme4
    {
        get => PlayerPrefs.GetInt("Theme4", 0) == 1;
        set => PlayerPrefs.SetInt("Theme4", value ? 1 : 0);
    }
    
    public static bool Theme5
    {
        get => PlayerPrefs.GetInt("Theme5", 0) == 1;
        set => PlayerPrefs.SetInt("Theme5", value ? 1 : 0);
    }
    
    public static bool Theme6
    {
        get => PlayerPrefs.GetInt("Theme6", 0) == 1;
        set => PlayerPrefs.SetInt("Theme6", value ? 1 : 0);
    }
    
    public static bool Theme7
    {
        get => PlayerPrefs.GetInt("Theme7", 0) == 1;
        set => PlayerPrefs.SetInt("Theme7", value ? 1 : 0);
    }
    
    public static bool Theme8
    {
        get => PlayerPrefs.GetInt("Theme8", 0) == 1;
        set => PlayerPrefs.SetInt("Theme8", value ? 1 : 0);
    }
    
    public static bool Theme9
    {
        get => PlayerPrefs.GetInt("Theme9", 0) == 1;
        set => PlayerPrefs.SetInt("Theme9", value ? 1 : 0);
    }
    
    public static int ThemeEquipped
    {
        get => PlayerPrefs.GetInt("ThemeEquipped", 0);
        set => PlayerPrefs.SetInt("ThemeEquipped", value);
    }
}
