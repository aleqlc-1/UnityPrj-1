using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    private Transform cameraContainer;

    private float rotateSemiAmount = 4;
    private float shakeAmount;

    private Vector3 startingLocalPos;

    void Start()
    {
        cameraContainer = GameObject.Find("CameraContainer").transform;
    }

    void Update()
    {
        if (shakeAmount > 0.01f)
        {
            Vector3 localPosition = startingLocalPos; // �ѹ� shake�Ҷ����� ī�޶��� �ʱ���ġ�� 0,0,0���� �ʱ�ȭ�ϸ鼭 ��������
            localPosition.x += shakeAmount * Random.Range(3, 5);
            localPosition.y += shakeAmount * Random.Range(3, 5);
            transform.localPosition = localPosition;
            shakeAmount = 0.9f * shakeAmount; // ���� ���ϰ� shake�ϸ鼭 shake����ǵ���
        }
    }

    public void Shake()
    {
        shakeAmount = Mathf.Min(0.1f, shakeAmount + 0.01f);
    }

    public void MediumShake()
    {
        shakeAmount = Mathf.Min(0.15f, shakeAmount + 0.015f);
    }

    public void RotateCameraToSide()
    {
        StartCoroutine(RotateCameraToSideRoutine());
    }

    private IEnumerator RotateCameraToSideRoutine()
    {
        int frames = 20;
        float increment = rotateSemiAmount / frames;

        for (int i = 0; i < frames; i++)
        {
            // Vector3.zero���� ������ Vector3.up���� �������� increment ������ŭ ������
            cameraContainer.RotateAround(Vector3.zero, Vector3.up, increment);
            yield return null;
        }

        yield break;
    }

    public void RotateCameraToFront()
    {
        StartCoroutine(RotateCameraToFrontRoutine());
    }

    private IEnumerator RotateCameraToFrontRoutine()
    {
        int frames = 60;
        float increment = rotateSemiAmount / frames;

        for (int i = 0; i < frames; i++)
        {
            // �����ܰ����� increment�� -���̰� frames�� 60���� �༭ �� �ܰ迡�� ȸ���ߴ� ��ŭ�� 1/3�� ����ġ�� �ǵ��ƿ�����
            cameraContainer.RotateAround(Vector3.zero, Vector3.up, -increment);
            yield return null;
        }

        cameraContainer.localEulerAngles = new Vector3(0, 0, 0); // ���ο� �ܰ谡 ���۵Ǹ� ī�޶� ������ �ٶ󺸵���

        yield break;
    }
}
