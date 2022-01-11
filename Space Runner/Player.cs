using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float jumpForce;
    private float speed = 150f;
    private float horizontal;
    private float vertical;

    private SpriteRenderer renderer;
    private Rigidbody2D myBody;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        myBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spikes") gameObject.SetActive(false);

        if (collision.tag == "Pickables") collision.gameObject.SetActive(false);
    }

    void Update()
    {
        // Input.GetAxis("Horizontal"); �ȴ����� 0, ���ʹ���Ű������ -1���� ����Ŀ��, �����ʹ���Ű������ 1���� ����Ŀ��
        // Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime; �ȴ����� 0, ���ʹ���Ű������ -0.04���� ����Ŀ��, �����ʹ���Ű������ 0.04���� ����Ŀ��
        // Input.GetAxis("Vertical"); �ȴ����� 0, �Ʒ��ʹ���Ű������ -1���� ����Ŀ��, ���ʹ���Ű������ 1���� ����Ŀ��
        horizontal = Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;
        vertical = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        // �ε� �Ҽ���(float)�� ����Ȯ�ϱ⿡ �񱳸� ���ؼ� == �����ں��ٴ� Mathf.Approximately�� ���ؾ���
        // ��, 1.0 == 10.0 / 10.0 �� true�� ��ȯ���� �������� ����
        // flipX�� üũ�ϸ� ���� ��������Ʈ�� �������� �ݴ�� ����
        // ��, ���ӽ��۽� ��������Ʈ�� �������� ���� �����Ƿ� ���ʹ���Ű�� ������ ������ �ٶ󺸵���
        if (!Mathf.Approximately(horizontal, 0)) renderer.flipX = (horizontal < 0);

        Vector3 targetVelocity = new Vector2(horizontal, myBody.velocity.y);
        Vector3 refVelocity = Vector3.zero;
        myBody.velocity = Vector3.SmoothDamp(myBody.velocity, targetVelocity, ref refVelocity, 0.03f);

        if (!Mathf.Approximately(vertical, 0)) // ������Ű �Ǵ� �Ʒ�����Ű ��������
        {
            if (Mathf.Approximately(myBody.velocity.y, 0)) // ���� �Ǵ� �ϰ����� �ƴϸ�
            {
                // XOR������(�� ���� ������ 0, �ٸ��� 1)
                // ��, �Ʒ��ʹ���Ű ������ myBody.gravityScale�� �����ų� ���ʹ���Ű ������ myBody.gravityScale�� ����̸�
                if (vertical < 0 ^ myBody.gravityScale > 0)
                {
                    myBody.gravityScale *= -1; // �߷� ���
                    renderer.flipY = (vertical > 0); // ��������Ʈ ���� ���
                }
            }
        }

        if (Mathf.Approximately(myBody.velocity.y, 0)) // ���Ʒ��� �������� �ʴ� ���¿���
        {
            if (Input.GetKey(KeyCode.Space)) // �����̽� ������
            {
                // gravityScale���� ���� ���� �����Ͽ� AddForce
                Vector3 direction = (myBody.gravityScale > 0) ? Vector3.up : Vector3.down;
                myBody.AddForce(jumpForce * direction, ForceMode2D.Impulse);
            }
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }
}
