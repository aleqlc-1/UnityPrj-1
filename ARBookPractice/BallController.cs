using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isReady = true;
    private Vector2 startPos;
    public float resetTime = 3.0f;

    public float captureRate = 0.3f; // ��ȹȮ�� 30%
    public Text result;

    public GameObject effect; // ����� ��ƼŬ

    void Start()
    {
        result.text = ""; // ��ȹ����ؽ�Ʈ�� �������� �ʱ�ȭ

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (isReady) return;

        float draw = Random.Range(0, 1.0f);

        if (draw <= captureRate)
        {
            result.text = "��ȹ ����!";

            // DB�� ����� ���¸� ��ȹ�� ���·� ����
            DB_Manager.instance.UpdateCaptured();
        }
        else
            result.text = "��ȹ�� ������ �����ƽ��ϴ�...";

        // ��ȹ�ߵ� ���ߵ� �������� ��ƼŬ����
        Instantiate(effect, collision.transform.position, Camera.main.transform.rotation);

        // ��ȹ�ߵ� ���ߵ� �������� ����� ĳ���͸� �����ϰ� ���� ��Ȱ��ȭ
        Destroy(collision.gameObject);
        gameObject.SetActive(false);
	}

	void Update()
    {
        if (!isReady) return;

        // ���� ī�޶� ���� �ϴܿ� ��ġ
        SetBallPosition(Camera.main.transform);

        if (Input.touchCount > 0 && isReady)
		{
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) // ��ġ�����ߴٸ�
            {
                startPos = touch.position; // ��ġ������ġ ����
			}
            else if (touch.phase == TouchPhase.Ended) // ��ġ������
			{
                // �հ����� �巡���� �ȼ��� Y�� �Ÿ��� ����
                float dragDistance = touch.position.y - startPos.y;

                // AR ī�޶� �������� ���� ����(���� 45�� ����)�� �����Ѵ�.
                Vector3 throwAngle = (Camera.main.transform.forward + Camera.main.transform.up).normalized;

                // ���ư��� ���� ����ȿ�� ����ǰ� ī�޶�տ��� �������� �ʵ���
                rb.isKinematic = false;
                isReady = false;

                // ForceMode.VelocityChange�� ������ٵ� ���� ������ �����ϰ� ���������� �ӵ��� ��ȭ�� �ִ� ���. �ӵ��� ���������� ����
                // ���� * �巡�װŸ���ŭ �����Գ��� * �ȼ������� ����ũ�⳷��
                rb.AddForce(throwAngle * dragDistance * 0.005f, ForceMode.VelocityChange);

                // 3�� �Ŀ� �� �ʱ�ȭ
                Invoke("ResetBall", resetTime);
			}
		}
    }

    private void SetBallPosition(Transform anchor)
	{
        Vector3 offset = anchor.forward * 0.5f + anchor.up * -0.2f;
        transform.position = anchor.position + offset;
	}

    private void ResetBall()
	{
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        isReady = true;
        gameObject.SetActive(true);
	}
}
