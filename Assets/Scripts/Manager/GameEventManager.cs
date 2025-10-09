using System;
using UnityEngine;

public static class GameEventManager
{
    public static Action CheckShapeDown;
    public static Action MoveShapeToStartPosition;
    public static Action SetShapeInactive;
    public static Action<Enemy> CheckNewWave;
    public static Action<bool> SoundStateChanged;
    public static Action<bool> MusicStateChanged;
    public static Action<bool> VibraStateChanged;
    public static Action<int> CoinCollected;
    public static Action<int> SwapCollected;
    public static Action UpdateShopResource;
    public static Action ReturnPoolObjects;
}
