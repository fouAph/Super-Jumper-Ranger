using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int credit = 400;
    public float buffDurationLeft { get; set; }
    public bool invicible { get; set; }
    public bool isPlayerDead { get; set; }
    public HealthSystem playerHealth { get; set; }
    public Character c { get; set; }
    public GameManager gm { get; set; }
    public UiManager uiManager { get; set; }
    public WeaponManager weaponManager { get; set; }
    private void Start()
    {
        Setup();
    }

    void Setup()
    {
        gm = GameManager.Singleton;
        uiManager = UiManager.Singleton;
        playerHealth = GetComponent<HealthSystem>();
        weaponManager = GetComponent<WeaponManager>();
        c = GetComponent<Character>();
        if (gm)
            gm.playerManager = this;
        if (!gm.isTesting)
            DisablePlayerController();
    }

    public void DisablePlayerController()
    {
        c.playerControlled = false;
    }

    public void EnablePlayerController()
    {
        c.playerControlled = true;
    }
}
