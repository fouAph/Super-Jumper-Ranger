using System.Collections.Generic;
using UnityEngine;

public class GunPickupRandom : MonoBehaviour
{
    public List<GunDataSO> gunDataSO;
    [SerializeField] ScoreAdder scoreAdder;

    private void Awake()
    {
        scoreAdder = GetComponent<ScoreAdder>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            WeaponManager.Singleton.PickupGun(this);
           

            if (GameManager.Singleton && scoreAdder)
            {
                GameManager.Singleton.UpdateScore(scoreAdder.scoreToAdd);
                GameManager.Singleton.currentBoxCount--;
            }
        }
    }


}
