using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player�� ������ٵ�2D Body Type�� Kinematic���� �Ͽ� �߷¿�������ʵ��� ��
// Body Type�� Dynamic���� �ϸ� �߷� ����Ǿ� �Ʒ��� ������
public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 200f;

    private float horizontal;

    public GameObject explosion;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // �¿����Ű�θ� �̵�

        // Space.World�� �ϸ� ��� Vector2.up�������� �������θ� �ö󰡰� �¿�Ű������ �� ���� �����Ͽ����� ȸ����
        // Space.Self�� �ؾ� �����Ʈ�� �ٲ�
        // Translate�Լ��� �̵��� ���� y���� �ʿ��ϹǷ� Vector2.up�ؼ� �̵��ѰŰ� rb.velocity�϶��� transform.up���� �̵��ؾ���
        // ����, �����̵��̹Ƿ� Time.deltaTime���ؼ� ���� ������. �Ȱ��ϸ� ��û����������
        transform.Translate(Vector2.up * speed * Time.deltaTime, Space.Self); // ������ �ڵ����� ����ǰԲ�

        transform.Rotate(Vector3.forward * -horizontal * rotationSpeed * Time.deltaTime);
    }
}
