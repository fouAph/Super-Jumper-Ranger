using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeItemSpawner : MonoBehaviour
{
    private Animator animator;
    [SerializeField] AudioClip spawnSfx;
    [System.NonSerialized] public bool busy;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SpawnItem()
    {
        animator.SetTrigger("Spawn");
        busy = true;
        StartCoroutine(PlaySpawnSFX());

        StartCoroutine(ResetBusy());
    }

    IEnumerator PlaySpawnSFX()
    {
        yield return new WaitForSeconds(.5f);
        AudioPoolSystem.Singleton.PlayAudioSFX(spawnSfx, 0.5f);

    }

    IEnumerator ResetBusy()
    {
        yield return new WaitForSeconds(1);
        busy = false;
    }
}
