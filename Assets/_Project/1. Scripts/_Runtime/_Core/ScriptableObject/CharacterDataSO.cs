using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterDataSO", menuName = "CharacterDataSO")]
public class CharacterDataSO : ScriptableObject
{
    public GameObject CharacterPrefab;
    public Sprite statsSprite;
    public string characterName;
    public float moveSpeed3 = 4f;
    public float maxJumpHeight =2.5f;
    public float damageMultiplier = 1f;

    public void SetCharacterStats()
    {
        var c = CharacterPrefab.GetComponent<Character>();
    }
}