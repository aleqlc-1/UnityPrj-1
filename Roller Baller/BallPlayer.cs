using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlayer : MonoBehaviour
{
    [SerializeField] private float moveForce = 20f;
    [SerializeField] private float maxAngularVelocity = 25f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float groundCheckRayLength = 1f;

    [SerializeField] private bool useTorque;

    private Rigidbody myBody;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        myBody.maxAngularVelocity = maxAngularVelocity; // ������ٵ��� �ִ� ���ӵ�. ������ ȸ���� �� ����� �Ҿ������� ���ϱ�����
    }

    public void Move(Vector3 moveDirection)
    {
        if (useTorque)
        {
            // �帮��Ʈ�ϵ��� �̵�
            // x�� z���� ����ؼ� �������ϴµ�?
            Debug.Log(new Vector3(moveDirection.z, 0f, -moveDirection.x));
            myBody.AddTorque(new Vector3(moveDirection.z, 0f, -moveDirection.x) * moveForce);
        }
        else
        {
            // �׳��̵�
            myBody.AddForce(moveDirection * moveForce);
        }
    }

    public void Jump(bool jump)
    {
        if (Physics.Raycast(transform.position, -Vector3.up, groundCheckRayLength) && jump)
        {
            myBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
