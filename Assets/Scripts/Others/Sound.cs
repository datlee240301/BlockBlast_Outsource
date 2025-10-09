using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string Name;

    public AudioClip AudioClip;
    [Range(0f, 1f)] public float Volume;

    public bool Loop;

    public AudioSource AudioSource;
}