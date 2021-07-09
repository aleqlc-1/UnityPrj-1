using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBox : MonoBehaviour
{
    [SerializeField] private Camera letterBox;


    private void Start()
	{
        SetAspect((float)9f, (float)16f);
    }
	/// <summary>
	/// ���͹ڽ��� ���� ���ϴ� ȭ�� ������ �������ִ� �Լ��Դϴ�.
	/// </summary>
	/// <param name="wRatio">���ϴ� ���� ����</param>
	/// <param name="hRatio">���ϴ� ���� ����</param>
	private void SetAspect(float wRatio, float hRatio)
    {
        //���� ī�޶��� ���� ������ ���� �޾ƿɴϴ�.
        Camera mainCam = Camera.main;

        //���ο� ȭ�� ũ�� 0f~1f�� ���� �����ϴ�.
        float newScreenWidth;
        float newScreenHeight;
        //���͹ڽ��� ũ�� 0f~1f�� ���� �����ϴ�.
        float letterWidth;
        float letterHeight;
        //���͹ڽ�. ���͹ڽ��� ȭ���� ���������ʴ� ī�޶� �������Դϴ�.
        Camera letterBox1 = Instantiate(letterBox);
        Camera letterBox2 = Instantiate(letterBox);

        //���ΰ� �� �� ������ ���ϴ� ���. ���Ϸ� ���͹ڽ��� ����ϴ�.
        if (wRatio > hRatio)
        {
            //���ο� ȭ���� ���� ũ��� ȭ���� �ִ�ġ
            newScreenWidth = 1f;
            //���� ũ��� ���θ� �������� ������ �����ݴϴ�(newScreenWidth : newScreenHeight = wRatio : hRatio)
            newScreenHeight = newScreenWidth / wRatio * hRatio;

            //���͹ڽ��� ���� ũ��� ���ο� ȭ���� ũ��� �����ϴ�.
            letterWidth = newScreenWidth;
            //���� ũ��� ���� ȭ�� ũ��(1f)���� ���ο� ȭ�� ũ�⸦ �� ũ���Դϴ�.
            //��, �Ʒ� �ΰ����� ����Ƿ� 2�� �������ݴϴ�.
            letterHeight = (1f - newScreenHeight) * 0.5f;

            //camera.rect�� ���� �Ʒ��κ��� 0f,0f�Դϴ�.
            //���ο� ũ���� ȭ���� �Ҵ����ݴϴ�. ȭ���� ������x�� 0, y�� �Ʒ� ���͹ڽ��� �������Դϴ�.
            mainCam.rect = new Rect(0f, letterHeight, newScreenWidth, newScreenHeight);

            letterBox1.rect = new Rect(0f, 0f, letterWidth, letterHeight);//�Ʒ� ���͹ڽ�
            letterBox2.rect = new Rect(0f, 1f - letterHeight, letterWidth, letterHeight);//�� ���͹ڽ�
        }
        //���ΰ� �� �� ������ ���ϴ� ���. �¿�� ���͹ڽ��� ����ϴ�. �������� ����մϴ�.
        else
        {
            newScreenHeight = 1f;
            newScreenWidth = newScreenHeight / hRatio * wRatio;

            letterHeight = newScreenHeight;
            letterWidth = (1f - newScreenWidth) * 0.5f;


            mainCam.rect = new Rect(letterWidth, 0f, newScreenWidth, newScreenHeight);

            letterBox1.rect = new Rect(0f, 0f, letterWidth, letterHeight);//���� ���͹ڽ�
            letterBox2.rect = new Rect(1f - letterWidth, 0f, letterWidth, letterHeight);//������ ���͹ڽ�
        }
        //���͹ڽ��� ���ڰ� ī�޶��� �ڽ����� �������ݴϴ�
        letterBox1.transform.parent = mainCam.transform;
        letterBox2.transform.parent = mainCam.transform;
    }
}
