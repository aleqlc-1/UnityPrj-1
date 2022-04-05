using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMotor : MonoBehaviour
{
    public float gravityMultiplier = 1f;
    public float lerpTime = 10f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetDirection = Vector3.zero;
    private float fallVelocity = 0f;

    [HideInInspector] public CharacterController charController;

    public float distanceToGround = 0.1f;

    private bool isGrounded;

    private Collider myCollider;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        myCollider = GetComponent<Collider>(); // CharacterController�� ��ü������ ĸ���ݶ��̴��� ����������.
    }

    void Start()
    {
        // extents�� �ݶ��̴� size�� ����ũ��
        // ���Ϳ��� ���ݸ�ŭ �������� ���Ͽ�
        distanceToGround = myCollider.bounds.extents.y;
    }

    void Update()
    {
        isGrounded = OnGroundCheck();

        moveDirection = Vector3.Lerp(moveDirection, targetDirection, Time.deltaTime * lerpTime);
        moveDirection.y = fallVelocity;

        charController.Move(moveDirection * Time.deltaTime);

        if (!isGrounded) fallVelocity -= 90f * gravityMultiplier * Time.deltaTime;
    }

    public bool OnGroundCheck()
    {
        RaycastHit hit;

        if (charController.isGrounded)
        {
            return true;
        }

        // center�� �ݶ��̴��� �߽ɰ�
        // distanceToGround + 0.1f�� ���Ϳ��� ���ݸ�ŭ �����ͼ� 0.1 �� �������� ���� ����
        if (Physics.Raycast(myCollider.bounds.center, -Vector3.up, out hit, distanceToGround + 0.1f))
        {
            return true;
        }

        return false;
    }

    public void Move(Vector3 dir)
    {
        targetDirection = dir;
    }

    public void Stop()
    {
        moveDirection = Vector3.zero;
        targetDirection = Vector3.zero;
    }

    public void Jump(float jumpSpeed)
    {
        if (isGrounded)
        {
            fallVelocity = jumpSpeed;
        }
    }
}
