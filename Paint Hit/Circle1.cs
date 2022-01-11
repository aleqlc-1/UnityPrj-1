using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle1 : MonoBehaviour
{
    void Start()
    {
        iTween.MoveTo(base.gameObject, iTween.Hash(new object[]
        {
            "y",
            0,
            "easetype",
            iTween.EaseType.easeInCirc,
            "time",
            0.2,
            "OnComplete",
            "RotateCircle" // iTween������ ȣ��� �޼����
        }));
    }

    void Update()
    {
        // y���� �������� ���������� ȸ��
        transform.Rotate(Vector3.up * BallHandler.rotationSpeed * Time.deltaTime);
    }

    private void RotateCircle()
    {
        print("The iTween anim is done");
    }
}
