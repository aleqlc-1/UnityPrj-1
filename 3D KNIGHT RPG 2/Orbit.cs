using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ʈ�� ������ ��ũ��Ʈ�� �ƴ����� transform.position ���Ƿ� MonoBehaviour ����ؾ���
public class Orbit : MonoBehaviour
{
    public SphericalVector spherical_Vector_Data = new SphericalVector(0, 0, 1);

    protected virtual void Update()
    {
        transform.position = spherical_Vector_Data.Position;
    }
}
