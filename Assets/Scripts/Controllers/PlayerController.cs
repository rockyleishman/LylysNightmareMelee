using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour, IHitPoints
{
    private Vector2 _moveInput;
    private GameObject CursorObject;
    private bool _isUsingMouse;
    private bool _isAttackReady;
    private bool _isContinualAttackReady;

    private InputAction _attackInputAction;

    public GameObject PlayerDirectionObject;

    private void Start()
    {
        //init HP
        InitHP();

        //get cursor reference
        CursorObject = DataManager.Instance.PlayerDataObject.CursorObject;

        //init private booleans
        _isUsingMouse = false;
        _isAttackReady = true;
        _isContinualAttackReady = true;

        //get button actions for held input
        _attackInputAction = GetComponent<PlayerInput>().actions.FindAction("Attack");
    }

    private void Update()
    {
        Movement();
        if (_isUsingMouse)
        {
            LookAtCursor();
        }

        ContinualAttack();
    }

    private void Movement()
    {
        //clamp diagonal movement
        if (_moveInput.magnitude > 1.0f)
        {
            _moveInput.Normalize();
        }

        //apply movement
        Vector3 movement = _moveInput * DataManager.Instance.PlayerDataObject.MovementSpeed * DataManager.Instance.PlayerDataObject.MovementSpeedMultiplier * Time.deltaTime;
        transform.Translate(movement);

        //try trail of assurance
        EventManager.Instance.TrailOfAssuranceTriggered.TriggerEvent(movement);
    }

    private void ContinualAttack()
    {
        foreach (ButtonControl buttonControl in _attackInputAction.controls)
        {
            if (buttonControl.isPressed && _isContinualAttackReady && _isAttackReady)
            {
                EventManager.Instance.PillowAttackTriggered.TriggerEvent(transform.position);
                StartCoroutine(AttackCooldown());
                StartCoroutine(ContinualAttackCooldown());
            }
        }
    }

    private void LookAtCursor()
    {
        //set cursor object to mouse cursor position
        Vector3 mouseCursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CursorObject.transform.position = new Vector3(mouseCursorPosition.x, mouseCursorPosition.y, 0.0f);

        //set player direction
        PlayerDirectionObject.transform.eulerAngles = new Vector3(PlayerDirectionObject.transform.eulerAngles.x, PlayerDirectionObject.transform.eulerAngles.y, Mathf.Atan2(CursorObject.transform.position.y - transform.position.y, CursorObject.transform.position.x - transform.position.x) * 180.0f / Mathf.PI + 90.0f);
    }

    private IEnumerator AttackCooldown()
    {
        _isAttackReady = false;

        yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.PillowAttackCooldown / DataManager.Instance.PlayerDataObject.CooldownDivisor);

        _isAttackReady = true;
    }

    private IEnumerator ContinualAttackCooldown()
    {
        _isContinualAttackReady = false;

        yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.PillowAttackContinuousCooldown / DataManager.Instance.PlayerDataObject.CooldownDivisor);

        _isContinualAttackReady = true;
    }

    #region InputMethods

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void OnAimWithJoystick(InputValue value)
    {
        _isUsingMouse = false;

        Vector2 inputValue = value.Get<Vector2>();

        //rotate direction object
        if (inputValue.magnitude > 0.0f)
        {
            PlayerDirectionObject.transform.eulerAngles = new Vector3(PlayerDirectionObject.transform.eulerAngles.x, PlayerDirectionObject.transform.eulerAngles.y, Mathf.Atan2(inputValue.y, inputValue.x) * 180.0f / Mathf.PI + 90.0f);
        }        

        //set cursor based on player direction
        CursorObject.transform.position = transform.position - PlayerDirectionObject.transform.up * DataManager.Instance.PlayerDataObject.DefaultCursorDistance;
    }

    private void OnAimWithMouse(InputValue value)
    {
        _isUsingMouse = true;

        //position handled in update to account for camera movement
    }

    private void OnAttack()
    {
        if (_isAttackReady)
        {
            EventManager.Instance.PillowAttackTriggered.TriggerEvent(transform.position);
            StartCoroutine(AttackCooldown());
        }

        //secondary attacks handle their own cooldowns
        EventManager.Instance.SecondaryAttackTriggered.TriggerEvent(transform.position);
    }

    private void OnSpecialAttack()
    {
        if (DataManager.Instance.PlayerDataObject.SpecialCharge >= 1.0f)
        {
            EventManager.Instance.SpecialAttackTriggered.TriggerEvent(transform.position);
            DataManager.Instance.PlayerDataObject.SpecialCharge = 0.0f;
        }
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
        EventManager.Instance.PlayerDamaged.TriggerEvent(transform.position);
        //TODO: trigger particle effect
    }

    public void OnDeath()
    {
        //TODO: trigger game over
    }

    #endregion
}
