using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float minDistance = 1f;
    private float distance;
    [SerializeField] private float minMoveSpeed = 0.5f, maxMoveSpeed = 2f;
    private float moveSpeed;

    private Transform playerTarget;

    void Start()
    {
        SetMoveSpeed();
    }

    void Update()
    {
        if (!playerTarget) return;

        transform.LookAt(playerTarget); // ����Ƽ�󿡼� x,y,z���� �״���ְ� transform�� ȸ������ ���ϰ� Ÿ���� �ٶ󺸴� ���� forward�̵���

        distance = Vector3.Distance(transform.position, playerTarget.position);

        // Enemy�� �÷��̾� ����
        if (distance > minDistance) transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void SetMoveSpeed()
    {
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    public void SetTarget(Transform target)
    {
        playerTarget = target;
    }
}
