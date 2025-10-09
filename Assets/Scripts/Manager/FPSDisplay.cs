using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    #region =========================== PROPERTIES ===========================

    [Header("FPS Settings")]
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] float _targetFrameRate = 60.0f;
    private int _maxRate = 9999;
    private float _currentFrameTime;
    
    private float _pollingTime = 0.5f;
    private float _time;
    private int _frameCount;

    #endregion

    #region =========================== UNITY CORES ===========================

    private void Start()
    {
        StartCoroutine(WaitForNextFrame());
#if !UNITY_EDITOR
            _fpsText.gameObject.SetActive(false);
#endif
    }
    
    private void Update()
    {
        _time += Time.deltaTime;
        _frameCount++;

        if (_time >= _pollingTime)
        {
            int frameRate = Mathf.RoundToInt(_frameCount/_time);
            _fpsText.text = $"FPS: {frameRate}";

            _time -= _pollingTime;
            _frameCount = 0;
        }
    }

    #endregion

    #region =========================== MAIN ===========================

    private IEnumerator WaitForNextFrame()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _maxRate;
        _currentFrameTime = Time.realtimeSinceStartup;
        while (true)
        {
            yield return new WaitForEndOfFrame();
            _currentFrameTime += 1.0f / _targetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = _currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < _currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }
    
    #endregion
}
