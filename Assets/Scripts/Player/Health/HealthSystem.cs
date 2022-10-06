using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;

    public GameObject deathVFX;
    public AudioClip deathClipSFX;
    [SerializeField] Color hitColor = Color.white;
    protected int currentHealth;
    SpriteRenderer spriteRenderer;
    protected Color originialColor;
    public UnityEvent onDeathEvent;

    [System.NonSerialized]
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
    }
    private void OnDisable()
    {
        onDeathEvent.RemoveAllListeners();
    }
    void DelayStart()
    {
        if (deathVFX)
        {
            PoolSystem.Singleton.AddObjectToPooledObject(deathVFX, 10);
            onDeathEvent.AddListener(delegate { OnDeath(); });
        }
    }

    public void SetMaxHealth()
    {
        currentHealth = maxHealth;
    }

    public void SetHealth(int value)
    {
        currentHealth = value;
    }

    public void OnDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, currentHealth);
        if (currentHealth == 0)
        {
            onDeathEvent?.Invoke();
            return;
        }
        if (gameObject.activeInHierarchy)
            StartCoroutine(EnemyBlinkRoutine());

    }

    IEnumerator EnemyBlinkRoutine()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = originialColor;
    }
    public void OnDeath()
    {
        PoolSystem.Singleton.SpawnFromPool(deathVFX, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        if (deathClipSFX)
            AudioPoolSystem.Singeleton.PlayAudioAtLocation(deathClipSFX, transform.position, 1f);
    }

    public void Die()
    {
        onDeathEvent?.Invoke();
    }
}

