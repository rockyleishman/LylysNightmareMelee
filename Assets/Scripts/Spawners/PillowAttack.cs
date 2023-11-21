using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowAttack : MonoBehaviour
{
    [SerializeField] public Animator PillowSwingAnimator;
    private bool IsRightSwing;
    private List<EnemyAIController> _enemiesHit;

    private void Start()
    {
        //start inactive
        gameObject.SetActive(false);

        //start with right swing
        IsRightSwing = true;

        //init enemies hit list
        _enemiesHit = new List<EnemyAIController>();
    }

    public void Attack()
    {
        //clear enemies hit list
        _enemiesHit.Clear();

        //activate and deactivate
        gameObject.SetActive(true);
        StopCoroutine(TimedDeactivation());
        StartCoroutine(TimedDeactivation());

        //animate attack
        AnimateAttack();
    }

    private void AnimateAttack()
    {
        //set animator right/left swing
        PillowSwingAnimator.SetBool("IsRightSwing", IsRightSwing);

        //set animator values based on rotation
        int rotationValue = Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.Player.PlayerDirectionObject.transform.eulerAngles.z / 22.5f);
        if (IsRightSwing)
        {
            switch (rotationValue)
            {
                case 1:
                case 2:
                    PillowSwingAnimator.SetBool("IsDiagonal", true);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    break;

                case 3:
                case 4:
                    PillowSwingAnimator.SetBool("IsDiagonal", false);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                    break;

                case 5:
                case 6:
                    PillowSwingAnimator.SetBool("IsDiagonal", true);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                    break;

                case 7:
                case 8:
                    PillowSwingAnimator.SetBool("IsDiagonal", false);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                    break;

                case 9:
                case 10:
                    PillowSwingAnimator.SetBool("IsDiagonal", true);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                    break;

                case 11:
                case 12:
                    PillowSwingAnimator.SetBool("IsDiagonal", false);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                    break;

                case 13:
                case 14:
                    PillowSwingAnimator.SetBool("IsDiagonal", true);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                    break;

                case 15:
                case 16:
                case 0:
                default:
                    PillowSwingAnimator.SetBool("IsDiagonal", false);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
            }
        }
        else
        {
            switch (rotationValue)
            {
                case 2:
                case 3:
                    PillowSwingAnimator.SetBool("IsDiagonal", true);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    break;

                case 4:
                case 5:
                    PillowSwingAnimator.SetBool("IsDiagonal", false);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                    break;

                case 6:
                case 7:
                    PillowSwingAnimator.SetBool("IsDiagonal", true);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                    break;

                case 8:
                case 9:
                    PillowSwingAnimator.SetBool("IsDiagonal", false);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                    break;

                case 10:
                case 11:
                    PillowSwingAnimator.SetBool("IsDiagonal", true);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                    break;

                case 12:
                case 13:
                    PillowSwingAnimator.SetBool("IsDiagonal", false);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                    break;

                case 14:
                case 15:
                    PillowSwingAnimator.SetBool("IsDiagonal", true);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                    break;

                case 16:
                case 0:
                case 1:
                default:
                    PillowSwingAnimator.SetBool("IsDiagonal", false);
                    PillowSwingAnimator.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
            }
        }

        //swap swing direction
        IsRightSwing = !IsRightSwing;

        //trigger animation
        PillowSwingAnimator.SetTrigger("OnSwing");
    }

    private IEnumerator TimedDeactivation()
    {
        //delay
        yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.PillowAttackDuration);

        //deactivate
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EnemyAIController enemy = other.GetComponent<EnemyAIController>();

        if (enemy != null && !_enemiesHit.Contains(enemy))
        {
            enemy.DamageHP(DataManager.Instance.PlayerDataObject.PillowAttackDamage * DataManager.Instance.PlayerDataObject.DamageMultiplier);

            _enemiesHit.Add(enemy);
        }
    }
}
