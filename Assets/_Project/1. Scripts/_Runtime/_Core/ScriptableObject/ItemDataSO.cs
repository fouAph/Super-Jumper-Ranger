using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDataSO", menuName = "ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public GameObject ItemPrefab;
    public string itemName;
    public Sprite itemSprite;
}
