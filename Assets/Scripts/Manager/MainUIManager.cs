using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : Singleton<MainUIManager>
{
    public bool PopupOpened;
    private static SceneTransition Scene => Instance?.GetComponent<SceneTransition>();
    
    public static void LoadScene(string sceneName)
    {
        Scene.PerformTransition(sceneName);
    }
}
