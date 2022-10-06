using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Singleton;
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("More than one Instance of " + Singleton.GetType());
            Debug.LogWarning("Destroying " + Singleton.name);
            DestroyImmediate(Singleton.gameObject);
        }

        Singleton = this;

    }

    public ItemSpawnManager itemSpawnManager;
    public float itemSpawnerWaitTime = 10;
    private float itemSpawnerTimer;

    public EnemySpawnerManager enemySpawnerManager;
}
