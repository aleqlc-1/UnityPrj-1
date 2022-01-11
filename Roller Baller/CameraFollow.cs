using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTarget;

    [SerializeField] private float distance = 10f;
    [SerializeField] private float cameraHeight = 5f;
    [SerializeField] private float heightDamping = 2f;
    private float wantedHeight, currentHeight;

    private Quaternion currentRotation;

    void Awake()
    {
        playerTarget = GameObject.FindWithTag(TagManager.PLAYER_TAG).transform;
    }

    void LateUpdate()
    {
        if (!playerTarget) return;

        // ī�޶��� ����
        wantedHeight = playerTarget.position.y + cameraHeight;
        currentHeight = transform.position.y;
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // ���̾��Űâ�� �Էµ� ī�޶��� ȸ�� y��(�÷��̾ �ٶ󺸴� ����)
        currentRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        transform.position = playerTarget.position; // �ϴ� ī�޶�����ġ = �÷��̾�����ġ
        transform.position -= currentRotation * Vector3.forward * distance; // �÷��̾��� ��ġ���� (-8.6, 0.0, -5.1)�� ���� ���� �Ҵ�
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z); // ���̸� �缳��

        transform.LookAt(playerTarget);
    }
}
