using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using Unity.Collections;
using UnityEngine.UI;

public class FindDetection : MonoBehaviour
{
    public ARFaceManager afm;
    public GameObject smallCube;

    List<GameObject> faceCubes = new List<GameObject>();

    ARCoreFaceSubsystem subSys;

    NativeArray<ARCoreFaceRegionData> regionData; // using Unity.Collections;

    public Text vertexIndex;

    void Start()
    {
		for (int i = 0; i < 3; i++)
		{
            GameObject go = Instantiate(smallCube);
            faceCubes.Add(go);
            go.SetActive(false);
		}

        // AR Face Manager�� ���� �ν��� �� ������ �Լ��� ����
        // �޽ù�� ������ OnDetectFaceAll�� ����
        afm.facesChanged += OnDetectThreePoints;

        // AR Foundation�� XRFaceSubsystem Ŭ���� ������ AR Core�� ARCoreFaceSubsystem Ŭ���� ������ ĳ����
        subSys = (ARCoreFaceSubsystem)afm.subsystem;
    }

    // facesChanged ��������Ʈ(Action)�� ������ �Լ�
    private void OnDetectThreePoints(ARFacesChangedEventArgs args)
	{
        // ���ν������� ���ŵȰ��� �ִٸ�(���� �������̶��)
        if (args.updated.Count > 0)
		{
            // �νĵ� �󱼿��� Ư�� ��ġ�� �����´�
            subSys.GetRegionPoses(args.updated[0].trackableId, Allocator.Persistent, ref regionData);

            // �νĵ� Ư�� ������ ��ġ�� ť�긦 �̵�
			for (int i = 0; i < regionData.Length; i++)
			{
                faceCubes[i].transform.position = regionData[i].pose.position;
                faceCubes[i].transform.rotation = regionData[i].pose.rotation;
                faceCubes[i].SetActive(true);
			}
		}

        // ���ν������� �Ҿ��ٸ�(�������� �ȵǰ� �ִٸ�)
        if (args.removed.Count > 0)
		{
            // ť�� ��Ȱ��ȭ
			for (int i = 0; i < regionData.Length; i++)
			{
                faceCubes[i].SetActive(false);
			}
		}
	}

    // �� �޽� �����͸� �̿��� ���
    private void OnDetectFaceAll(ARFacesChangedEventArgs args)
	{
        if (args.updated.Count > 0)
		{
            // +-��ư���� �ؽ�Ʈ UI�� ���� ���ڿ� �����͸� ������ �����ͷ� ��ȯ�Ѵ�.
            int num = int.Parse(vertexIndex.text);

            // �� ���� �迭���� ������ �ε����� �ش��ϴ� ��ǥ�� �����´�
            Vector3 vertPosition = args.updated[0].vertices[num];

            // ������ǥ�� ������ǥ�� ��ȯ�Ͽ� vertPosition�� ����
            vertPosition = args.updated[0].transform.TransformPoint(vertPosition);

            // �غ�� ť�� �ϳ��� Ȱ��ȭ�ϰ� ���� ��ġ�� ������ ���´�.
            faceCubes[0].SetActive(true);
            faceCubes[0].transform.position = vertPosition;
		}
        else if (args.removed.Count > 0)
		{
            faceCubes[0].SetActive(false);
		}
	}
}
