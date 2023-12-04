using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedTimeObject : PoolObject
{
    protected IEnumerator LifeTimer(float lifeTime)
    {
        yield return new WaitForSecondsRealtime(lifeTime);

        OnDespawn();
    }
}
