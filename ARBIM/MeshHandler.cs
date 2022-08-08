using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeshHandler : MonoBehaviour
{
	[SerializeField] private GameObject buildingOnPlane;

	//// ������ؽ�Ʈ
	//public TextMeshProUGUI touchBeganTxt;
	//public TextMeshProUGUI touch0Txt;
	//public TextMeshProUGUI touch1Txt;
	//public TextMeshProUGUI touchMovedTxt;

	void Start()
	{
		MeshRenderer[] childrenOnAir = buildingOnPlane.transform.GetComponentsInChildren<MeshRenderer>(); // root�� ����

		foreach (var child in childrenOnAir)
		{
			child.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}
	}

	public void OffShadowCast(GameObject tempObj)
	{
		MeshRenderer[] childrenOnPlane = tempObj.transform.GetComponentsInChildren<MeshRenderer>(); // root�� ����

		foreach (var child in childrenOnPlane)
		{
			child.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}
	}
}
