using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    public RectTransform extraBallInner;

    private GameController gameController;

    private float currentWidth, addWidth, totalWidth;

    void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        extraBallInner.sizeDelta = new Vector2(31, 117); // Width, Height �ʱⰪ ����
        currentWidth = 31; // ExtraBallInner ��ü�� ���̾��Űâ�� ������ �ʱⰪ
        totalWidth = 600; // ExtraBallInner ��ü�� Width�� 600�ָ� ������ ����
    }

    void Update()
    {
        // ������ ������ ������ �߻綧 �� 1�� �� �� �� �ֵ���
        if (currentWidth >= totalWidth)
        {
            gameController.ballsCount++; // �� 1�� �ø���
            gameController.ballsCountText.text = gameController.ballsCount.ToString();
            currentWidth = 31; // ������ �ʱ�ȭ
        }

        // currentWidth �ʱⰪ�� 31�̹Ƿ� ���ӽ��۵Ǹ� �ϴ� 36���� ������ ���� ����.
        if (currentWidth >= addWidth)
        {
            addWidth += 5;
            extraBallInner.sizeDelta = new Vector2(addWidth, 117); // ������ �������� ä��
        }
        // ������ 36���� ���� else �������Դٰ� addWidth�� 31�� �Ǽ� �ٽ� if������ ��. ��ϱ����� currentWidth�� Ŀ���� +-5 �ϸ� ��� �ݺ�
        else
            addWidth = currentWidth;
    }

    // ��� 1�� ���������� ä�������� ������ �ִ밪 �÷��� �����ϴ� �޼���
    // addRandom�� ������ �������� ������. �������� ���� 600���� ä��� update������ ������ �߻綧 ���� 1�� �� �� �� �ֵ���
    public void IncreaseCurrentWidth()
    {
        // currentWidth % 576f�� currentWidth�� �����ϱ� ����
        // ������ currentWidth�� 576�϶� �� ���� ������ �������� ������ �پ�������ٵ�
        int addRandom = Random.Range(80, 120);
        currentWidth = currentWidth % 576f + addRandom + 31;
    }
}
