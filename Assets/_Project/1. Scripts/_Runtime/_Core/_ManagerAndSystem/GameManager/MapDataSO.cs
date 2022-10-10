using UnityEngine;

[CreateAssetMenu(fileName = "new MapData", menuName = "MapDataSO")]
public class MapDataSO : ScriptableObject
{
    public string mapName;
    public int maxEnemyCountToSpawn;
    public int maxBoxToSpawn;
    public int scoreTarget;

    // public 
}
