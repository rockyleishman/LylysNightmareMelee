using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] public Effect PlayerHitEffect;
    [SerializeField] public Effect PlayerDeathEffect;
    [SerializeField] public Effect EnemyHitEffect;
    [SerializeField] public Effect EnemyDeathEffect;

    public void TriggerPlayerHitEffect(Vector3 position)
    {
        //spawn effect
        Effect effect = (Effect)PoolManager.Instance.Spawn(PlayerHitEffect.name, position, Quaternion.identity);
        effect.transform.SetParent(transform);

        //TODO: play audio

    }
    public void TriggerPlayerDeathEffect(Vector3 position)
    {
        //spawn effect
        Effect effect = (Effect)PoolManager.Instance.Spawn(PlayerDeathEffect.name, position, Quaternion.identity);
        effect.transform.SetParent(transform);

        //TODO: play audio

    }

    public void TriggerEnemyHitEffect(Vector3 position)
    {
        //spawn effect
        Effect effect = (Effect)PoolManager.Instance.Spawn(EnemyHitEffect.name, position, Quaternion.FromToRotation(Vector3.down, position - DataManager.Instance.PlayerDataObject.Player.transform.position));
        effect.transform.SetParent(transform);

        //TODO: play audio

    }
    public void TriggerEnemyDeathEffect(Vector3 position)
    {
        //spawn effect
        Effect effect = (Effect)PoolManager.Instance.Spawn(EnemyDeathEffect.name, position, Quaternion.FromToRotation(Vector3.down, position - DataManager.Instance.PlayerDataObject.Player.transform.position));
        effect.transform.SetParent(transform);

        //TODO: play audio

    }
}
