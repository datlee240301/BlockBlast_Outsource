using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NotEnoughCoinPopup : MonoBehaviour
{
    private readonly WaitForSeconds _wait = new WaitForSeconds(2.2f);
    private float _timeToReachTarget = 0.5f;
    private Vector3 _targetScale = new Vector3(1.2f, 1.2f, 1.2f);
    
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(TurnOff());
    }
    
    private IEnumerator TurnOff()
    {
        transform.DOScale(_targetScale, _timeToReachTarget).SetEase(Ease.OutBounce);
        yield return _wait;
        GameUIManager.Instance.SetSortingOrder(1);
        gameObject.SetActive(false);
    }
}
