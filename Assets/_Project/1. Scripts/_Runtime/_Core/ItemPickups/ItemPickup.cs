using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class ItemPickup : MonoBehaviour
{
    public ItemDataSO itemDataSO;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // itemDataSO.OnPickedupItem();
            gameObject.SetActive(false);
        }
    }
}