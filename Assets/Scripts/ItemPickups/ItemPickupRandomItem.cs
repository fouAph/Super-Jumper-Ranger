using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class ItemPickupRandomItem : MonoBehaviour
{
    public List<ItemDataSO> itemDatas;
   
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // WeaponManager.Singleton.PickupGun(this);
            // gameObject.SetActive(false);
            print($"pickup {itemDatas[Random.Range(0, itemDatas.Count)].ItemPrefab.name}");

           
        }
    }
}
