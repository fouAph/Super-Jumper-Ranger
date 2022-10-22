using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : WeaponBase
{
    public AudioClip shootClip;
    public Transform bulletSpawnPoint;
    public ParticleSystem particle;
    public float fuel = 100;
    public float fuelConsumptionMultiplier;
    public bool isFiring;
    public int DamagePerSecond = 20;
    GameManager gm;
    bool fireInput;
    public CapsuleCollider2D col2D;
    [SerializeField] private bool debugSetGunPosition;
    AudioSource source;
    private float currentFuel;
    private void Awake()
    {
        gm = GameManager.Singleton;
        if (source == null)
        {
            source = new GameObject("flamethrowerAudio").AddComponent<AudioSource>();
            source.volume = AudioPoolSystem.Singleton.shootVolume * AudioPoolSystem.Singleton.masterVolume;
            source.transform.SetParent(transform);
        }
        currentFuel = (int)fuel;
        currentAmmo = (int)currentFuel;
    }

    private void Update()
    {
        if (gm.useMobileControll)
        {
            fireInput = gm.mobileController.shootJoystick.progress >= gm.mobileController.shootThreshold;
        }
        else
        {
            if (gunDataSO)
                fireInput = gunDataSO.autoFire ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
            else fireInput = Input.GetKey(KeyCode.Mouse0);
        }
        if (fireInput && currentAmmo >= 0 || gm.mobileController.isFiring)
        {
            Shoot();


        }
        else
        {
            particle.Stop();
            source.Stop();
            col2D.enabled = false;
        }
        if (debugSetGunPosition)
        {
            Vector3 pos = transform.localPosition;
            gunDataSO.spawnPosition = pos;
        }
    }

    public void Shoot()
    {
        if (!source.isPlaying)
        {
            source.loop = true;
            source.PlayOneShot(shootClip);
        }
        currentFuel -= Time.deltaTime * fuelConsumptionMultiplier;
        currentAmmo = (int)currentFuel;
        UiManager.Singleton.UpdateAmmoCountText(currentAmmo);
        particle.Play();
        col2D.enabled = true;
    }
}
