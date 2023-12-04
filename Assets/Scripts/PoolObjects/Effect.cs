using System.Collections;
using UnityEngine;

public class Effect : LimitedTimeObject
{
    [SerializeField] float Lifespan = 1.0f;

    private void OnEnable()
    {
        StartCoroutine(LifeTimer(Lifespan));
    }
}
