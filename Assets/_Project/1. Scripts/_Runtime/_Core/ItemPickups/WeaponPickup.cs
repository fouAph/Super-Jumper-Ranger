using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WeaponPickup : MonoBehaviour
{
    public WeaponDataSO gunDataSO;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.collider.GetComponent<WeaponManager>().PickupGun(this);

        }
    }
}
