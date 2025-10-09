using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Color color = new Color32(235, 171, 189, 255);
    
    public void PerformTransition(string scene)
    {
        Transition.LoadLevel(scene, duration, color);
    }
}
