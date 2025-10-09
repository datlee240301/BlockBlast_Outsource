using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _timeToReachTarget = 1.5f;
    
    public void Move()
    {
        _target = GameUIManager.Instance.Target;
        transform.DOMove(_target.position, _timeToReachTarget).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            GameUIManager.Instance.UpdateHealth(1);
            gameObject.SetActive(false);
        });
    }
}
