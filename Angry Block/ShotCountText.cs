using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotCountText : MonoBehaviour
{
    public AnimationCurve scaleCurve;

    public CanvasGroup shotCount;

    private CanvasGroup cg;

    private Text topText, bottomText;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        topText = transform.Find("TopText").GetComponent<Text>();
        bottomText = transform.Find("BottomText").GetComponent<Text>();
        transform.localScale = Vector3.zero;
    }

    public void SetTopText(string text)
    {
        topText.text = text;
    }

    public void SetBottomText(string text)
    {
        bottomText.text = text;
    }

    public void Flash()
    {
        cg.alpha = 1; // �ʿ�����ڵ�
        transform.localScale = Vector3.zero;
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        for (int i = 0; i <= 60; i++)
        {
            // �ִϸ��̼� 0�ʴ��� ������ 1�ʴ��� ������ localScale�� ��ȭ��Ŵ
            transform.localScale = Vector3.one * scaleCurve.Evaluate((float)i / 50);

            // 40�϶��� localScale���� �����ְ� ���� ���������ٰ� i�� 60�϶� �����
            if (i >= 40) cg.alpha = (float)(60 - i) / 20;

            yield return null;
        }

        yield break;
    }
}
