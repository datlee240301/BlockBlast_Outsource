using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private bool _isArrow = true;
    [SerializeField] private TrailRenderer trailRenderer;
    private Vector3 _direction = Vector3.up;
    
    private void Update()
    {
        transform.position += _direction * (_speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Enemy"))
        {
            ResourceManager.Score += 10;
            GameUIManager.Instance.UpdateScore();
            if (this._isArrow)
            {
                gameObject.SetActive(false);
            }
            other.GetComponent<Collider2D>().enabled = false;
        }
    }
}
