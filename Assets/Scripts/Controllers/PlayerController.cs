using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IHitPoints
{
    [SerializeField] public float MovementSpeed = 5.0f;
    private Vector2 _moveInput;

    private void FixedUpdate()
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
        transform.Translate(_moveInput * MovementSpeed * Time.deltaTime);
    }

    #region InputMethods

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        Debug.Log(_moveInput);
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
        //TODO: attack
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

        //TODO: trigger effect
    }

    public void OnDeath()
    {
        //TODO: trigger game over
    }

    #endregion
}
