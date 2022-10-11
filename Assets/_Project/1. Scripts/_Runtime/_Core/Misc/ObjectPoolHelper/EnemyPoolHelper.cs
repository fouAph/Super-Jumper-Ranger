using UnityEngine;
public class EnemyPoolHelper : PoolHelper
{
    public HealthSystem healthSystem;
    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
    }
    public override void OnObjectSpawn()
    {
        healthSystem.Setup();
        base.OnObjectSpawn();

    }
}