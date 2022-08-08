using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ��ġ�� UI�� �̸� ���, Tag�� Ư�� UI�� ���а���
public class GraphicRaycast : MonoBehaviour
{
	[SerializeField] private GraphicRaycaster gr;
	[SerializeField] private TMPro.TextMeshProUGUI text;

	void Update()
	{
		var touchZero = Input.GetTouch(0);

		var pointer_event_data = new PointerEventData(null);
		pointer_event_data.position = touchZero.position;
		List<RaycastResult> resultList = new List<RaycastResult>();
		gr.Raycast(pointer_event_data, resultList);

		if (resultList.Count > 0)
		{
			GameObject rayedObj = resultList[0].gameObject;

			if (rayedObj.layer == LayerMask.NameToLayer("UI"))
			{
				text.text = rayedObj.name;
			}
		}
	}
}