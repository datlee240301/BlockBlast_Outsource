using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Type1,
        Type2,
    }

    public EnemyType Type;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _type1, _type2;
    [SerializeField] private Animator _animator;
    private readonly WaitForSeconds _wait = new WaitForSeconds(1f);
    private Collider2D _collider;
    
    private void Awake()
    {
        this._collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        ChangeCol(false);
        // StartCoroutine(Attack());
    }
    
    public void ChangeCol(bool value)
    {
        this._collider.enabled = value;
    }

    public void StartAttack(){
        StartCoroutine(Attack());
    }

    public void SetType(int type)
    {
        Type = type % 2 == 0 ? EnemyType.Type1 : EnemyType.Type2;
        _spriteRenderer.sprite = Type == EnemyType.Type1 ? _type1 : _type2;
        _animator.Play(Type == EnemyType.Type1 ? "Idle" : "Idle2");
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (!MainUIManager.Instance.PopupOpened){
                var num = Random.Range(20f, 30f);
                yield return new WaitForSeconds(num);
                ObjectPooler.Instance.SpawnFromPool("Egg", transform.position);
            }
            else{
                yield return null;
            }
        }
    }

    public void Return()
    {
        transform.position = ObjectPooler.Instance.Root.position;
        transform.SetParent(ObjectPooler.Instance.Root);
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arrow"))
        {
            DeathAnim();
            GameEventManager.CheckNewWave?.Invoke(this);
        }
        else if (other.CompareTag("Rocket"))
        {
            DeathAnim();
            GameEventManager.CheckNewWave?.Invoke(this);
        }
    }

    private void DeathAnim()
    {
        AudioManager.PlaySound("Chicken");
        transform.DOKill();
        transform.SetParent(ObjectPooler.Instance.Root);
        _animator.Play("Death");
        StartCoroutine(Death());
    }
    
    private IEnumerator Death()
    {
        yield return _wait;
        gameObject.SetActive(false);
    }
}
