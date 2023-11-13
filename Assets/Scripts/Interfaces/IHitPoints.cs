public interface IHitPoints
{
    public void InitHP();

    public void HealHP(float hp);

    public void DamageHP(float hp);

    public void OnDeath();
}