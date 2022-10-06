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
        AudioPoolSystem.Singeleton.PlayAudio(spawnSfx, 0.5f);
        busy = true;

        StartCoroutine(ResetBusy());
    }

    IEnumerator ResetBusy()
    {
        yield return new WaitForSeconds(1);
        busy = false;
    }
}
