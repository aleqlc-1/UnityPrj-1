using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boss Spawn FX ������Ʈ�� ������ü�鿡 ������ ��ũ��Ʈ
public class UnscaledTimeParticleSystem : MonoBehaviour
{
    private ParticleSystem particleFX;

    void Awake()
    {
        particleFX = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // ������ �����ɶ� ��ƼŬ ȿ�� �����ֱ�����
        if (Time.timeScale < 0.01f)
        {
            // ������ �ð� ���� ��ƼŬ�� �ùķ��̼��Ͽ� ��ƼŬ �ý����� ���� ���Ҵٰ� �Ͻ� ������
            // Time.unscaledDeltaTime�� Time.timeScale�� ������ ���� ����
            // �ι�°���� true�� ��� �ڽİ�ü�� ��������
            // ����°���� false�� ������� ����
            particleFX.Simulate(Time.unscaledDeltaTime, true, false);
        }
    }
}
