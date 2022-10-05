using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeItemSpawner : MonoBehaviour
{
    private Animator animator;
    public bool busy;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SpawnItem()
    {
        animator.SetTrigger("Spawn");
        busy = true;
    StartCoroutine(ResetBusy());
    }

    IEnumerator ResetBusy()
    {
        yield return new WaitForSeconds(1);
        busy = false;
    }
}
