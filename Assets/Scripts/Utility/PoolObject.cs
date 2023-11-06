using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public void OnDespawn()
    {
        PoolManager.Instance.Despawn(this);
    }
}