using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {

        HealthSystem healthSystem = other.collider.GetComponent<HealthSystem>();
        if (healthSystem)
        {
            healthSystem.Die();
        }

    }
}

