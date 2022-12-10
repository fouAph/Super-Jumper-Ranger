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

        upgradeLevel_TMP.text = $"Upgrade Level {tempWeapon.currentDamageLevelUpgrade}/{weaponDataSO.upgradeStats.maxDamageLevelUpgrades.Length}";
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

            upgradeOrBuyTMP.text = $"Upgrade for {weaponDataSO.upgradeStats.damageUpgradeLevelPrice[tempWeapon.currentDamageLevelUpgrade]}";
            // bought = true;
        }
    }
    public void OnUpgradeWeapon(PlayerManager playerManager)
    {
        bool fullyUpgraded = false;
        // GameManager.Singleton.playerManager.weaponManager.UpgradeDamage(weaponDataSO.itemName);
        if (fullyUpgraded == false)
        {
            gm.tempSavedWeaponStats[weaponDataSO.itemName].curentDamage += weaponDataSO.upgradeStats.maxDamageLevelUpgrades[gm.tempSavedWeaponStats[weaponDataSO.itemName].currentDamageLevelUpgrade];
            tempWeapon.curentDamage += weaponDataSO.upgradeStats.maxDamageLevelUpgrades[tempWeapon.currentDamageLevelUpgrade];
            var temp = tempWeapon;
            gm.tempSavedWeaponStats[weaponDataSO.itemName] = temp;

            playerManager.gm.tempSavedWeaponStats[weaponDataSO.itemName].currentDamageLevelUpgrade++;
            upgradeLevel_TMP.text = $"Upgrade Level {tempWeapon.currentDamageLevelUpgrade}/{weaponDataSO.upgradeStats.maxDamageLevelUpgrades.Length}";
            if (temp.currentDamageLevelUpgrade >= weaponDataSO.upgradeStats.maxDamageLevelUpgrades.Length)
            {
                fullyUpgraded = true;
            }
        }
        else
        {
            upgradeLevel_TMP.text = $"Fully Upgraded";
            BuyButton.interactable = false;
        }
    }


}