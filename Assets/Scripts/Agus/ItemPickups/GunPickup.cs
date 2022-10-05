using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GunPickup : MonoBehaviour
{
    public GunDataSO gunDataSO;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
            WeaponManager.Singleton.PickupGun(this);
    }
}
