using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �ֵθ��� �ִϸ��̼��ǿ��� �̺�Ʈ ����� ���
public class AttackDamageController : MonoBehaviour
{
    public GameObject attackPoint_1;
    public GameObject attackPoint_2;

    private void Turn_On_AttackPoint_1()
    {
        attackPoint_1.SetActive(true);
    }

    private void Turn_Off_AttackPoint_1()
    {
        if (attackPoint_1.activeInHierarchy) attackPoint_1.SetActive(false);
    }

    private void Turn_On_AttackPoint_2()
    {
        attackPoint_2.SetActive(true);
    }

    private void Turn_Off_AttackPoint_2()
    {
        if (attackPoint_2.activeInHierarchy) attackPoint_2.SetActive(false);
    }
}
