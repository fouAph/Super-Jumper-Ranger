using TMPro;
public class WeaponShopItem : ShopItem
{
    public WeaponDataSO weaponDataSO;
    public TMP_Text upgradeLevel_TMP;
    public int currentUpgradeLevel = 0;
    public int maxUpgradeLevel;
    private WeaponBase tempWeapon;

    private GameManager gm;
    void Start()
    {
        gm = GameManager.Singleton;
        itemName = weaponDataSO.itemName;
        itemName_TMP.text = weaponDataSO.itemName;
        tempWeapon = gm.tempSavedWeaponStats[weaponDataSO.itemName];

        // upgradeOrBuyTMP.text = $"Buy for {price.ToString()}";

        upgradeLevel_TMP.text = $"Upgrade Level {gm.savedCurrentLevelUpgrade[weaponDataSO.itemName]}/{weaponDataSO.upgradeStats.maxDamageLevelUpgrades.Length}";
        if (itemImage) itemImage.sprite = weaponDataSO.itemSprite;

    }
    public override void OnBuyItem(PlayerManager playerManager)
    {
        base.OnBuyItem(playerManager);
        if (bought)
        {
            if (!playerManager.gm.unlockedWeapons.Contains(weaponDataSO))
            {
                playerManager.gm.unlockedWeapons.Add(playerManager.gm.GetWeaponDataSoFromDict(weaponDataSO.itemName));
                OnBuyWeapon(playerManager);
            }

            else
            {
                OnUpgradeWeapon(playerManager);
            }
        }
    }

    public void OnBuyWeapon(PlayerManager playerManager)
    {
        if (playerManager.gm.CompareWeaponDataSoFromDict(weaponDataSO))
        {

            upgradeOrBuyTMP.text = $"Upgrade for {weaponDataSO.upgradeStats.damageUpgradeLevelPrice[gm.savedCurrentLevelUpgrade[weaponDataSO.itemName]]}";
            // bought = true;
        }
    }
    public void OnUpgradeWeapon(PlayerManager playerManager)
    {
        
        bool fullyUpgraded = false;
        // GameManager.Singleton.playerManager.weaponManager.UpgradeDamage(weaponDataSO.itemName);
        if (fullyUpgraded == false)
        {
            if (gm.savedCurrentLevelUpgrade.ContainsKey(weaponDataSO.itemName))
            {
                gm.savedCurrentLevelUpgrade[weaponDataSO.itemName]++;

            }
            // playerManager.gm.tempSavedWeaponStats[weaponDataSO.itemName].currentDamageLevelUpgrade++;
            upgradeLevel_TMP.text = $"Upgrade Level {gm.savedCurrentLevelUpgrade[weaponDataSO.itemName]}/{weaponDataSO.upgradeStats.maxDamageLevelUpgrades.Length}";

            if (gm.savedCurrentLevelUpgrade[weaponDataSO.itemName] >= weaponDataSO.upgradeStats.maxDamageLevelUpgrades.Length)
            {
                fullyUpgraded = true;
                upgradeLevel_TMP.text = $"Fully Upgraded";
                BuyButton.interactable = false;
            }
            else
            {
                
                upgradeOrBuyTMP.text = $"Upgrade for {weaponDataSO.upgradeStats.damageUpgradeLevelPrice[gm.savedCurrentLevelUpgrade[weaponDataSO.itemName]]}";

            }

            foreach (var item in gm.savedCurrentLevelUpgrade)
            {
                print($"{item.Key} current Level Upgrade is {item.Value}");
            }
        }
        else
        {
            upgradeLevel_TMP.text = $"Fully Upgraded";
            BuyButton.interactable = false;
        }
    }


}