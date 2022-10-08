using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class WeaponManager : MonoBehaviour
{
    #region Singeleton
    public static WeaponManager Singleton;
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("More than 1 Instance");
        }
        Singleton = this;
    }
    #endregion

    public Weapon currentGun { get; set; }
    public int maxSlot = 1;
    public Transform weaponInventoryHolder;
    public bool useInfiniteAmmo;
    public bool useSwapWeapon;


    private int selectedWeapon;
    List<Weapon> guns = new List<Weapon>();
    UiManager uiManager;
    private void Start()
    {
        uiManager = UiManager.Singleton;
        weaponInventoryHolder = GameObject.FindGameObjectWithTag("WeaponHolder").transform;
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
                currentGun = gun.GetComponentInChildren<Weapon>();
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
            guns.Add(pickupDataSo.ItemPrefab.GetComponent<Weapon>());

            if (currentGun)
                gun.SetActive(false);

            _gunpickup.gameObject.SetActive(false);
        }

        // EquipAvailableWeapon();
        SelectWeapon();

    }

    public void PickupGun(WeaponPickupRandom _gunPickupRandom)
    {
        bool isPickedup = false;
        var pickupDataSo = _gunPickupRandom.gunDataSO;

        //FIXME Fix Swap Weapon Algorithm
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
            int randGun = 0;
            randGun = Random.Range(0, pickupDataSo.Count);

            var gun = Instantiate(pickupDataSo[randGun].ItemPrefab, weaponInventoryHolder.position, weaponInventoryHolder.rotation);
            gun.transform.SetParent(weaponInventoryHolder);
            guns.Add(pickupDataSo[randGun].ItemPrefab.GetComponent<Weapon>());

            if (currentGun)
                gun.SetActive(false);

            _gunPickupRandom.gameObject.SetActive(false);
        }

        // EquipAvailableWeapon();
        SelectWeapon();

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
