using UnityEngine;

[CreateAssetMenu(fileName = "new MapData", menuName = "MapDataSO")]
public class MapDataSO : ScriptableObject
{
    public int indexInBuildIndex;
    public bool unlocked;
    public int maxEnemyCount;
    public int maxBoxCount;
    public int scoreTarget;

    public GameObject[] Enemies;
    public WeaponPickupRandom[] weaponPickupRandoms;
}
