using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneSleepMode : MonoBehaviour
{
    void Start()
    {
        // ������� �Է¾�� ������� �ȵǵ���
        // Screen.sleepTimeout = 30; �̷��Ծ��� 30�ʰ� �Է¾����� �������
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
