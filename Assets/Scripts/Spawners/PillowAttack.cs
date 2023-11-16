using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowAttack : MonoBehaviour
{
    [Header("Pillow Swing Effects")]
    [SerializeField] public GameObject SAndSSE;
    [SerializeField] public GameObject SSEAndSE;
    [SerializeField] public GameObject SEAndESE;
    [SerializeField] public GameObject ESEAndE;
    [SerializeField] public GameObject EAndENE;
    [SerializeField] public GameObject ENEAndNE;
    [SerializeField] public GameObject NEAndNNE;
    [SerializeField] public GameObject NNEAndN;
    [SerializeField] public GameObject NAndNNW;
    [SerializeField] public GameObject NNWAndNW;
    [SerializeField] public GameObject NWAndWNW;
    [SerializeField] public GameObject WNWAndW;
    [SerializeField] public GameObject WAndWSW;
    [SerializeField] public GameObject WSWAndSW;
    [SerializeField] public GameObject SWAndSSW;
    [SerializeField] public GameObject SSWAndS;
    private bool IsRightSwing;

    private void Start()
    {
        //start inactive
        gameObject.SetActive(false);

        //start with right swing
        IsRightSwing = true;
    }

    public void Attack()
    {
        //activate
        gameObject.SetActive(true);

        //activate pillow swing effects & start delayed deactivation
        float rotationDegrees = DataManager.Instance.PlayerDataObject.Player.PlayerDirectionObject.transform.eulerAngles.z;
        if (IsRightSwing)
        {
            if (rotationDegrees >= 11.25 && rotationDegrees < 56.25)
            {
                //SSE/SE
                SSEAndSE.SetActive(true);
                StartCoroutine(DelayedDeactivation(SSEAndSE));
            }
            else if (rotationDegrees >= 56.25 && rotationDegrees < 101.25)
            {
                //ESE/E
                ESEAndE.SetActive(true);
                StartCoroutine(DelayedDeactivation(ESEAndE));
            }
            else if (rotationDegrees >= 101.25 && rotationDegrees < 146.25)
            {
                //ENE/NE
                ENEAndNE.SetActive(true);
                StartCoroutine(DelayedDeactivation(ENEAndNE));
            }
            else if (rotationDegrees >= 146.25 && rotationDegrees < 191.25)
            {
                //NNE/N
                NNEAndN.SetActive(true);
                StartCoroutine(DelayedDeactivation(NNEAndN));
            }
            else if (rotationDegrees >= 191.25 && rotationDegrees < 236.25)
            {
                //NNW/NW
                NNWAndNW.SetActive(true);
                StartCoroutine(DelayedDeactivation(NNWAndNW));
            }
            else if (rotationDegrees >= 236.25 && rotationDegrees < 281.25)
            {
                //WNW/W
                WNWAndW.SetActive(true);
                StartCoroutine(DelayedDeactivation(WNWAndW));
            }
            else if (rotationDegrees >= 281.25 && rotationDegrees < 326.25)
            {
                //WSW/SW
                WSWAndSW.SetActive(true);
                StartCoroutine(DelayedDeactivation(WSWAndSW));
            }
            else
            {
                //SSW/S
                SSWAndS.SetActive(true);
                StartCoroutine(DelayedDeactivation(SSWAndS));
            }
        }
        else
        {
            if (rotationDegrees >= 33.75 && rotationDegrees < 78.75)
            {
                //SE/ESE
                SEAndESE.SetActive(true);
                StartCoroutine(DelayedDeactivation(SEAndESE));
            }
            else if (rotationDegrees >= 78.75 && rotationDegrees < 123.75)
            {
                //E/ENE
                EAndENE.SetActive(true);
                StartCoroutine(DelayedDeactivation(EAndENE));
            }
            else if (rotationDegrees >= 123.75 && rotationDegrees < 168.75)
            {
                //NE/NNE
                NEAndNNE.SetActive(true);
                StartCoroutine(DelayedDeactivation(NEAndNNE));
            }
            else if (rotationDegrees >= 168.75 && rotationDegrees < 213.75)
            {
                //N/NNW
                NAndNNW.SetActive(true);
                StartCoroutine(DelayedDeactivation(NAndNNW));
            }
            else if (rotationDegrees >= 213.75 && rotationDegrees < 258.75)
            {
                //NW/WNW
                NWAndWNW.SetActive(true);
                StartCoroutine(DelayedDeactivation(NWAndWNW));
            }
            else if (rotationDegrees >= 258.75 && rotationDegrees < 303.75)
            {
                //W/WSW
                WAndWSW.SetActive(true);
                StartCoroutine(DelayedDeactivation(WAndWSW));
            }
            else if (rotationDegrees >= 303.75 && rotationDegrees < 348.75)
            {
                //SW/SSW
                SWAndSSW.SetActive(true);
                StartCoroutine(DelayedDeactivation(SWAndSSW));
            }
            else
            {
                //S/SSE
                SAndSSE.SetActive(true);
                StartCoroutine(DelayedDeactivation(SAndSSE));
            }
        }

        //swap swing direction
        IsRightSwing = !IsRightSwing;        
    }

    private IEnumerator DelayedDeactivation(GameObject swingEffect)
    {
        //delay
        yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.PillowAttackActiveTime);

        //deactivate pillow swing effect
        swingEffect.SetActive(false);

        //deactivate
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAIController enemy = other.GetComponent<EnemyAIController>();

        if (enemy != null)
        {
            enemy.DamageHP(DataManager.Instance.PlayerDataObject.PillowAttackDamage * DataManager.Instance.PlayerDataObject.DamageMultiplier);
        }
    }
}
