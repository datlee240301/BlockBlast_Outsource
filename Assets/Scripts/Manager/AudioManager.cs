using System.Collections;
using System.Collections.Generic;
using CandyCoded.HapticFeedback;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
     #region =========================== PROPERTIES ===========================

    [SerializeField] private Sound[] _sounds;

    public static bool IsSoundEnable
    {
        get => PlayerPrefs.GetInt("IsSoundEnable", 1) == 1;
        set
        {
            if (value == IsSoundEnable)
            {
                return;
            }

            PlayerPrefs.SetInt(nameof(IsSoundEnable), value ? 1 : 0);
            GameEventManager.SoundStateChanged?.Invoke(value);
        }
    }

    public static bool IsMusicEnable
    {
        get => PlayerPrefs.GetInt("IsMusicEnable", 1) == 1;
        set
        {
            if (value == IsMusicEnable)
            {
                return;
            }

            PlayerPrefs.SetInt("IsMusicEnable", value ? 1 : 0);
            GameEventManager.MusicStateChanged?.Invoke(value);
        }
    }

    public static bool IsVibrationEnable
    {
        get => PlayerPrefs.GetInt("IsVibrationEnable", 1) == 1;
        set
        {
            if (value == IsVibrationEnable)
            {
                return;
            }

            PlayerPrefs.SetInt("IsVibrationEnable", value ? 1 : 0);
            GameEventManager.VibraStateChanged?.Invoke(value);
        }
    }

    #endregion

    #region =========================== UNITY CORES ===========================

    private void Start()
    {
        GameEventManager.MusicStateChanged += OnMusicStateChanged;
        GameEventManager.SoundStateChanged += OnSoundStateChanged;
        GameEventManager.VibraStateChanged += OnVibraStateChanged;
        foreach (Sound s in _sounds)
        {
            s.AudioSource = gameObject.AddComponent<AudioSource>();
            s.AudioSource.clip = s.AudioClip;
            s.AudioSource.loop = s.Loop;
            s.AudioSource.volume = s.Volume;
        }
    }

    #endregion

    #region =========================== MAIN ===========================

    public static void PlaySound(string name)
    {
        if (IsSoundEnable)
        {
            foreach (Sound s in Instance._sounds)
            {
                if (s.Name == name)
                    s.AudioSource.PlayOneShot(s.AudioClip);
            }
        }
    }

    public static void PlayLoopSound(string name)
    {
        if (IsMusicEnable)
        {
            foreach (Sound s in Instance._sounds)
            {
                if (s.Name == name)
                    if (!s.AudioSource.isPlaying)
                        s.AudioSource.Play();
            }
        }
    }

    public static void StopMusic()
    {
        foreach (Sound s in Instance._sounds)
        {
            if (s.Name == "MainTheme")
                s.AudioSource.Stop();
        }
    }

    public static void StopSound()
    {
        foreach (Sound s in Instance._sounds)
        {
            if (s.Name != "MainTheme")
                s.AudioSource.Stop();
        }
    }

    public static void PlayVibration()
    {
        if (IsVibrationEnable)
        {
            Handheld.Vibrate();
        }
    }
    
    public static void LightFeedback()
    {
        if (IsVibrationEnable)
        { 
            HapticFeedback.LightFeedback();
        }
    }

    public void OnMusicStateChanged(bool state)
    {
        if (state)
            PlayLoopSound("MainTheme");
        else
            StopMusic();
    }

    public void OnSoundStateChanged(bool state)
    {
        if (state)
            PlaySound("Click");
        else
            StopSound();
    }

    public void OnVibraStateChanged(bool state)
    {
        if (state)
            PlayVibration();
        else return;
    }

    #endregion
}
