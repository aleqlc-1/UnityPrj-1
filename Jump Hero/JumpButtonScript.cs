using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

// Jump Button�� ������ ��ũ��Ʈ
public class JumpButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // ��ư ������ ȣ��, �� ��ũ��Ʈ�� UI ������Ʈ�� �����Ǿ������� ȣ���
    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerJumpScript.instance != null) PlayerJumpScript.instance.SetPower(true);
    }

    // ��ư ���� ȣ��, �� ��ũ��Ʈ�� UI ������Ʈ�� �����Ǿ������� ȣ���
    public void OnPointerUp(PointerEventData eventData)
    {
        if (PlayerJumpScript.instance != null) PlayerJumpScript.instance.SetPower(false);
    }
}
