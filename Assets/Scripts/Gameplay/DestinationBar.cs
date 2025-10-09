using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationBar : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Egg"))
        {
            other.gameObject.GetComponent<Egg>().PlayAnimation();
            GameUIManager.Instance.UpdateHealth(-1);
        }
    }
}
