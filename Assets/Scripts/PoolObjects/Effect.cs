using System.Collections;
using UnityEngine;

public class Effect : PoolObject
{
    [SerializeField] float Lifespan = 1.0f;

    private void OnEnable()
    {
        StartCoroutine(LifeTimer());
    }

    private IEnumerator LifeTimer()
    {
        yield return new WaitForSecondsRealtime(Lifespan);

        OnDespawn();
    }
}
