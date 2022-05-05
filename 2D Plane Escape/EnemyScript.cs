using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 200f;

    private Transform playerTarget;

    private Rigidbody2D myBody;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        playerTarget = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (playerTarget == null) return;

        // (Vector2)�� ĳ�����Ͽ� z�� ����
        Vector2 point2Target = (Vector2)transform.position - (Vector2)playerTarget.position;
        point2Target.Normalize();

        float value = Vector3.Cross(point2Target, transform.up).z;

        myBody.velocity = transform.up * speed; // Time.deltaTime���ϸ� ��û������ ��. �ӵ� �� ��ü�̱� ������ �״�� �Ҵ�
        myBody.angularVelocity = value * rotationSpeed;
    }
}
