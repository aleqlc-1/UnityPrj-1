using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle5 : MonoBehaviour
{
    void Start()
    {
        iTween.MoveTo(base.gameObject, iTween.Hash(new object[]
        {
            "y",
            0,
            "easetype",
            iTween.EaseType.easeInOutQuad,
            "time",
            0.6,
            "OnComplete",
            "RotateCircle" // iTween������ ȣ��� �޼����
        }));
    }

    private void RotateCircle()
    {
        // 1�ʵ��� ȸ���ϰ� 1�� �����ٰ� �ݴ�� 1�ʵ��� ȸ�� �ݺ�
        iTween.RotateBy(base.gameObject, iTween.Hash(new object[]
        {
            "y",
            1f,
            "time",
            BallHandler.rotationTime,
            "easeType",
            iTween.EaseType.easeInOutQuad,
            "loopType",
            iTween.LoopType.pingPong,
            "delay",
            1
        }));
    }
}
