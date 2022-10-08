using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class HealthSystem : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    public int currentHealth;

    //OnDamaged Settings
    [SerializeField] Color hitColor = Color.white;
    protected SpriteRenderer spriteRenderer;
    protected Color originialColor;
    [SerializeField] private AudioClip damagedSFX;
    public UnityEvent onDamagedEvent;

    //Death Settings
    public GameObject deathVFX;
    public AudioClip deathClipSFX;
    public UnityEvent onDeathEvent;

    public bool isDead;

    //TODO make individual HealthSystem For Enemy and Player
    public virtual void Start()
    {
        isDead = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        SetMaxHealth();

        if (spriteRenderer)
            originialColor = spriteRenderer.color;

        Invoke("DelayStart", .2f);

        onDeathEvent.AddListener(delegate { OnDeath(); });
        onDamagedEvent.AddListener(delegate { OnDamaged(); });
    }

    void DelayStart()
    {
        if (deathVFX && PoolSystem.Singleton)
        {
            PoolSystem.Singleton.AddObjectToPooledObject(deathVFX, 10);
        }
    }

    #region  Health Methods
    public void SetMaxHealth()
    {
        currentHealth = maxHealth;
    }

    public void SetHealth(int value)
    {
        currentHealth = value;
    }
    #endregion

    #region Interaface Methode
    public void OnDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, currentHealth);
        UiManager.Singleton.UpdateHealth();

        if (currentHealth > 0)
            onDamagedEvent?.Invoke();
        else if (currentHealth == 0)
            onDeathEvent?.Invoke();

        if (gameObject.activeSelf)
            StartCoroutine(SpriteBlinkRoutine());

    }
    #endregion

    void OnDamaged()
    {
        AudioPoolSystem.Singleton.PlayAudio(damagedSFX, .5f);

    }

    public void OnDeath()
    {
        PoolSystem.Singleton.SpawnFromPool(deathVFX, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        if (deathClipSFX)
            AudioPoolSystem.Singleton.PlayAudioAtLocation(deathClipSFX, transform.position, 1f);
        isDead = true;
    }

    public void Die()
    {
        onDeathEvent?.Invoke();
    }

    IEnumerator SpriteBlinkRoutine()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = originialColor;
    }

}

