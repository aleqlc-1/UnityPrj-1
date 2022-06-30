using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public ARFaceManager faceManager;

	public Material[] faceMats;

	public Text indexText;
	private int vertNum = 0;
	private int vertCount = 468;

	void Start()
	{
		// ������ �ε������� 0���� �ʱ�ȭ
		indexText.text = vertNum.ToString();
	}

	// ��ư�������� ����� �Լ�
	public void ToggleMaskImage()
	{
		// faceManager ������Ʈ���� ���� ������ Face ������Ʈ�� ��� ��ȸ
		foreach (ARFace face in faceManager.trackables)
		{
			// ���� ���� �ν��ϰ� �ִ� ���¶��
			if (face.trackingState == TrackingState.Tracking)
			{
				// Face ������Ʈ�� Ȱ��ȭ�� ���
				face.gameObject.SetActive(!face.gameObject.activeSelf);
			}
		}
	}

	// ���͸��� ���� ��ư �Լ�
	public void SwitchFaceMaterial(int num)
	{
		foreach (ARFace face in faceManager.trackables)
		{
			if (face.trackingState == TrackingState.Tracking)
			{
				MeshRenderer mr = face.GetComponent<MeshRenderer>();
				mr.material = faceMats[num];
			}
		}
	}

	public void IndexIncrease()
	{
		// vertNum�� ���� 1 ������Ű��, �ִ��ε����� �����ʵ����Ѵ�.
		int number = Mathf.Min(++vertNum, vertCount - 1);
		indexText.text = number.ToString();
	}

	public void IndexDecrease()
	{
		// vertNum�� ���� 1 ���ҽ�Ű��, �ּ��ε������� �����ʵ����Ѵ�.
		int number = Mathf.Max(--vertNum, 0);
		indexText.text = number.ToString();
	}
}
