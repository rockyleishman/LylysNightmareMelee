using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IHitPoints
{
    private Vector2 _moveInput;

    public GameObject PlayerDirectionObject;

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        //clamp diagonal movement
        if (_moveInput.magnitude > 1.0f)
        {
            _moveInput.Normalize();
        }

        //apply movement
        transform.Translate(_moveInput * DataManager.Instance.PlayerDataObject.MovementSpeed * DataManager.Instance.PlayerDataObject.MovementSpeedMultiplier * Time.deltaTime);

        //rotate direction object
        if (_moveInput.magnitude > 0.0f)
        {
            PlayerDirectionObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(-_moveInput.x, -_moveInput.y));
        }
    }

    #region InputMethods

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void OnAimWithJoystick(InputValue value)
    {
        //TODO: aim cursor
    }

    private void OnAimWithMouse(InputValue value)
    {
        //TODO: aim cursor
    }

    private void OnAttack()
    {
        EventManager.Instance.PillowAttackTriggered.TriggerEvent(this, null);
    }

    private void OnSpecialAttack()
    {
        //TODO: special attack
    }

    private void OnPause()
    {
        //TODO: pause game
    }

    #endregion

    #region HitPointMethods

    public void InitHP()
    {
        DataManager.Instance.PlayerDataObject.CurrentHP = DataManager.Instance.PlayerDataObject.MaxHP;
    }

    public void HealHP(float hp)
    {
        float currentHP = DataManager.Instance.PlayerDataObject.CurrentHP + hp;

        if (currentHP > DataManager.Instance.PlayerDataObject.MaxHP)
        {
            DataManager.Instance.PlayerDataObject.CurrentHP = DataManager.Instance.PlayerDataObject.MaxHP;
        }
        else
        {
            DataManager.Instance.PlayerDataObject.CurrentHP = currentHP;
        }

        //TODO: trigger effect
    }

    public void DamageHP(float hp)
    {
        float currentHP = DataManager.Instance.PlayerDataObject.CurrentHP - hp;

        if (currentHP < 0.0f)
        {
            DataManager.Instance.PlayerDataObject.CurrentHP = 0.0f;
            OnDeath();
        }
        else
        {
            DataManager.Instance.PlayerDataObject.CurrentHP = currentHP;
        }

        //trigger effects
        EventManager.Instance.ScreenShakeTriggered.TriggerEvent(transform.position);
        //TODO: trigger particle effect
    }

    public void OnDeath()
    {
        //TODO: trigger game over
    }

    #endregion
}
