using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    [HideInInspector] public Vector3 movementDirection;

    private Rigidbody myBody;

    public float walk_Speed = 5f;
    public float walking_Force = 50f;
    public float turning_Smoothing = 0.1f;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HangleMovement();
    }

    private void HangleMovement()
    {
        Vector3 targetVelocity = movementDirection * walk_Speed; // �����ϰ����ϴ� Ÿ��velocity
        Vector3 deltaVelocity = targetVelocity - myBody.velocity; // Ÿ��velocity - ����velocity

        if (myBody.useGravity) deltaVelocity.y = 0f; // �߷�����ȴٸ� �ٴ��� �ɾ���ϹǷ� y���� 0����

        myBody.AddForce(deltaVelocity * walking_Force, ForceMode.Acceleration); // ForceMode.Acceleration�� ���ι����� ����

        // �̵��ϴ������� �ٶ󺸵����ϴ� ����
        Vector3 face_Direction = movementDirection;
        if (face_Direction == Vector3.zero)
            myBody.angularVelocity = Vector3.zero; // ĳ���� ȸ�� ����
        else
        {
            float rotationAngle = AngleAroundAxis(transform.forward, face_Direction, Vector3.up);

            myBody.angularVelocity = Vector3.up * rotationAngle * turning_Smoothing;
        }
    }


    private float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
        float angle = Vector3.Angle(dirA, dirB); // �� ���ͻ����� ����(����), 90�� �̳��� ���� ������

        // Vector3.Dot�� �� ���ͻ����� ����. �� ���� Mathf.Acos�� ���ڿ� ������ �� ���ͻ����� ����(����)�� ������
        // Vector3.Cross�� �� ���ͻ����� ����. ������ �� ���Ͱ� ������ �̷궧�� �ٸ� �������Ⱚ
        // ĳ���Ͱ� �ּ�ȸ���Ÿ��� ȸ���ϵ��� �ϴ� �����ε�
        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) > 0 ? 1 : -1);
    }
}
