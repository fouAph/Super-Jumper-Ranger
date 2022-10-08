using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region  Singleton
    public static PlayerManager Singleton;
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
    #endregion
    public CharacterDataSO characterDataSO;

    public GameObject PlayerObject;
    public HealthSystem playerHealth;
    public bool invicible;

    private void Start()
    {
        // Invoke("DelayStart", .1f);
        DelayStart();
    }

    void DelayStart()
    {
        Instantiate(GameManager.Singleton.currentCharacter.CharacterPrefab, transform.position, Quaternion.identity);

        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        playerHealth = PlayerObject.GetComponent<HealthSystem>();
    }




}
