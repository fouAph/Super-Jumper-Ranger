using System.Collections;
using UnityEngine;

public class ImpactPoolHelper : PoolHelper, IPooledObject
{
    public AudioClip impactSFX;
    private void Awake()
    {
        useDeactive = true;
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        AudioPoolSystem.Singleton.PlayAudioAtLocation(impactSFX, transform.position, 1f);
  
    }

    
}