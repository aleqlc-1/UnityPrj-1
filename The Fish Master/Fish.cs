using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Fish : MonoBehaviour
{
    [Serializable]
    public class FishType
    {
        public int price;

        public float fishCount;
        public float minLength;
        public float maxLength;
        public float colliderRadius;

        public Sprite sprite;
    }

    private FishType type;
    public FishType Type
    {
        get { return type; }
        set
        {
            type = value;
            coll.radius = type.colliderRadius;
            rend.sprite = type.sprite;
        }
    }

    private CircleCollider2D coll;

    private SpriteRenderer rend;

    private float screenLeft;

    private Tweener tweener;

    void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        rend = GetComponentInChildren<SpriteRenderer>();

        screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        Debug.Log(screenLeft); // -4�����ε� ������ �� -3�������� ����?
    }

    public void ResetFish()
    {
        if (tweener != null) tweener.Kill(false);

        coll.enabled = true;

        float num = UnityEngine.Random.Range(type.minLength, type.maxLength); // ������ ���� �������ϱ�
        Vector3 position = transform.position; // ������ġ��
        position.x = screenLeft; // ������ x��ǥ �Ҵ��ϰ�
        position.y = num; // ���� �Ҵ��Ͽ�
        transform.position = position; // ����

        float num2 = 1;
        float y = UnityEngine.Random.Range(num - num2, num + num2); // ������ -1,+1 �� ������ �������ϱ�
        Vector2 v = new Vector2(-position.x, y); // �밢���̵��ϵ��� �ݴ��� ��ġ ���ϱ�

        float num3 = 3; // �ݴ������ �����ϴµ� 3�ʰɸ�
        float delay = UnityEngine.Random.Range(0, 2 * num3); // �̵� ���� 0 ~ 5 ����, SetDelay�� tween�� ���۵Ǳ� �� �����̼���

        // SetLoops -1�� ���ѹݺ�, 1�� �ѹ��� ����
        tweener = transform.DOMove(v, num3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay).OnStepComplete(delegate
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        });
    }

    public void Hooked()
    {
        coll.enabled = false;
        tweener.Kill(false);
    }
}
