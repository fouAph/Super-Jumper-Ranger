using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class WeaponManager : MonoBehaviour
{
    public WeaponBase currentGun { get; set; }
    public int maxSlot = 1;
    public Transform weaponInventoryHolder;
    public bool useSwapWeapon;


    private int selectedWeapon;
    private List<WeaponBase> guns = new List<WeaponBase>();
    private UiManager uiManager;


    private void Start()
    {
        uiManager = UiManager.Singleton;
        if (!weaponInventoryHolder)
            Debug.LogWarningFormat("weaponInventoryHolder variable is not assigned on {0} object", gameObject.name);
       
    }

    private void Update()
    {
        ChangeWeaponSlotInput();
    }

    public void ChangeWeaponSlotInput()
    {
        // if (PlayerManager.instance.blockInput) { return; }
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (guns.Count > 0)
            {
                selectedWeapon = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (guns.Count > 1)
            {
                selectedWeapon = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (guns.Count > 2)
            {
                selectedWeapon = 2;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (guns.Count > 3)
            {
                selectedWeapon = 3;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (guns.Count > 4)
            {
                selectedWeapon = 4;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (guns.Count > 5)
            {
                selectedWeapon = 5;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (guns.Count > 6)
            {
                selectedWeapon = 6;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (guns.Count > 7)
            {
                selectedWeapon = 7;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (guns.Count > 8)
            {
                selectedWeapon = 8;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (guns.Count > 9)
            {
                selectedWeapon = 9;
            }
        }



        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //      DropWeapon(currentWeapon.weaponStats);
        // }


        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }

    }

    public void SelectWeapon()
    {
        int i = 0;

        foreach (Transform gun in weaponInventoryHolder)
        {
            if (i == selectedWeapon)
            {
                gun.gameObject.SetActive(true);
                currentGun = gun.GetComponentInChildren<WeaponBase>();
                if (currentGun)
                    StartCoroutine(UpdateAmmoRoutine());


                // CurrentEquip(curWeapon);
                // UIManager.instance.weaponSlotUIs[i].selectedPanel.gameObject.SetActive(true);
            }

            else
                gun.gameObject.SetActive(false);
            // UIManager.instance.weaponSlotUIs[i].selectedPanel.gameObject.SetActive(false);
            // UIManager.instance.ResetAlpha();
            i++;
        }

    }

    IEnumerator UpdateAmmoRoutine()
    {
        yield return new WaitForSeconds(.1f);
        uiManager.UpdateAmmoCountText(currentGun.currentAmmo);
    }

    public void PickupGun(WeaponPickup _gunpickup)
    {
        bool isPickedup = false;
        var pickupDataSo = _gunpickup.gunDataSO;

        if (guns.Count < maxSlot)
            isPickedup = true;

        if (isPickedup)
        {
            //TODO Spawn With PoolSystem
            var gun = Instantiate(pickupDataSo.ItemPrefab, weaponInventoryHolder.position, weaponInventoryHolder.rotation);
            gun.transform.SetParent(weaponInventoryHolder);
            guns.Add(pickupDataSo.ItemPrefab.GetComponent<WeaponBase>());

            if (currentGun)
                gun.SetActive(false);

            _gunpickup.gameObject.SetActive(false);
        }

        // EquipAvailableWeapon();
        SelectWeapon();

    }
    int randGun;
    public WeaponBase PickupGun(WeaponPickupRandom _gunPickupRandom)
    {
        GameObject weapon = null;
        bool isPickedup = false;
        var pickupDataSo = _gunPickupRandom.weaponDataSoToSpawn;

        //FIXME Fix Swap WeaponBase Algorithm
        if (useSwapWeapon && currentGun)
        {
            currentGun.transform.SetParent(PoolSystem.Singleton.transform);
            currentGun.gameObject.SetActive(false);
            currentGun = null;
            isPickedup = true;
            if (guns.Count > 0)
                guns.RemoveAt(0);
        }
        else
        {
            if (guns.Count < maxSlot)
                isPickedup = true;
        }

        if (isPickedup)
        {
            //TODO Spawn With PoolSystem
            StartCoroutine(RandomInt(0, pickupDataSo.Count));

            // var gun = Instantiate(pickupDataSo[randGun].ItemPrefab, weaponInventoryHolder.position, weaponInventoryHolder.rotation);
            weapon = PoolSystem.Singleton.SpawnFromPool(pickupDataSo[randGun].itemName, weaponInventoryHolder.position, weaponInventoryHolder.rotation);

            if (GameManager.Singleton && GameManager.Singleton.flipMode == CharacterFlipMode.ByMoveDirection)
            {
                Vector3 euler = Vector3.zero;
                Vector3 scale = Vector3.one;
                if (transform.localScale.x > 0)
                {
                    euler = weapon.transform.localEulerAngles = Vector3.zero;
                    scale = weapon.transform.localScale = Vector3.one;
                }
                else
                {
                    euler = weapon.transform.localEulerAngles = new Vector3(0, 180, 0);
                    scale = weapon.transform.localScale = new Vector3(1, 1, 1);
                }

                weapon.transform.localEulerAngles = euler;
                weapon.transform.localScale = scale;
            }
            weapon.transform.SetParent(weaponInventoryHolder);
            guns.Add(pickupDataSo[randGun].ItemPrefab.GetComponent<WeaponBase>());

            if (currentGun)
                weapon.SetActive(false);

            _gunPickupRandom.gameObject.SetActive(false);

        }

        // EquipAvailableWeapon();
        SelectWeapon();
        return weapon.GetComponent<Weapon>();


    }

    IEnumerator RandomInt(int targetRand, int max)
    {
        int randNumber = 0;
        int prev = randNumber;
        // print($"prev {prev}");

        while (randNumber == prev)
        {
            randNumber = Random.Range(0, max);
            if (randNumber != prev)
            {
                prev = randNumber;
                // print($"prev {prev}");
                // print($"randNumber {randNumber}");
                break;
            }
            //     print(randNumber);
            yield return null;
        }

        randGun = randNumber;
        yield return null;
    }


    #region Unused
    /*
    public void EquipAvailableWeapon()
    {
        if (currentGun == null)
        {
            for (int i = 0; i < guns.Count; i++)
            {
                if (guns[i])
                {
                    selectedWeapon = i;
                    // CurrentEquip(_weapon);
                    SelectWeapon();
                    // if (currentWeapon) { return; }
                }
            }
        }
    }
    */
    #endregion
}

[System.Serializable]
public class WeaponUpgradeTracker
{
    public string weaponId;
    public int currentLevelUpgrade = -1;
}