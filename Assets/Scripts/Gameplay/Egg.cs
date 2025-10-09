using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private Sprite _eggCrack, _normalEgg;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private readonly WaitForSeconds _wait = new WaitForSeconds(1f);

    private void Awake()
    {
        this._rb = GetComponent<Rigidbody2D>();
        this._spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        this._rb.gravityScale = 0.9f;
        this._spriteRenderer.sprite = _normalEgg;
    }

    public void PlayAnimation()
    {
        AudioManager.PlaySound("Egg");
        AudioManager.LightFeedback();
        this._spriteRenderer.sprite = _eggCrack;
        StartCoroutine(EggCrackAnim());
    }
    
    private IEnumerator EggCrackAnim()
    {
        yield return _wait;
        gameObject.SetActive(false);
    }
}
