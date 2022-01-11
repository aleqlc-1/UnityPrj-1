using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hook : MonoBehaviour
{
    public Transform hookedTransform;

    private Camera mainCamera;

    private Collider2D coll;

    private bool canMove;

    private int length;
    private int strength;
    private int fishCount;

    private Tweener cameraTween;

    private List<Fish> hookedFishes;

    void Awake()
    {
        mainCamera = Camera.main;
        coll = GetComponent<Collider2D>();
        hookedFishes = new List<Fish>();
    }

    void Update()
    {
        if (canMove && Input.GetMouseButton(0))
        {
            Vector3 vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = vector.x;
            transform.position = position;
        }
    }

    // ��ư ��üȭ�� ��� �����ϰ� ���� ȭ�� �ƹ����� Ŭ���ϸ� �� �޼��� ȣ��
    public void StartFishing()
    {
        length = IdleManager.instance.length - 20;
        strength = IdleManager.instance.strength;
        fishCount = 0;

        float time = (-length) * 0.1f; // 5

        // OnUpdate�� DOMoveY�� ������ OnComplete�� ȣ��Ǳ� ������ ����ؼ� ȣ���
        cameraTween = mainCamera.transform.DOMoveY(length, 1 * time * 0.25f, false).OnUpdate(delegate
        {
            Debug.Log("OnUpdate");
            if (mainCamera.transform.position.y <= -11)
                transform.SetParent(mainCamera.transform); // hook�� ī�޶��� �ڽİ�ü�� ����� ī�޶� ���󰡵���
        }).OnComplete(delegate
        {
            Debug.Log("OnComplete");
            coll.enabled = true; // �� ���������� ����� ��ƿ÷����ϹǷ� �ݶ��̴� Ȱ��ȭ
            cameraTween = mainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate
            {
                Debug.Log("OnUpdateInOnComplete");
                if (mainCamera.transform.position.y >= -25f)
                    StopFishing();
            });
        });

        Debug.Log("out");
        ScreensManager.instance.ChangeScreen(Screens.GAME);
        coll.enabled = false; // ��ư������ tween���۵ǰ� �ٷ� ���ڵ� ����ǹǷ� ��Ȱ��ȭ��ä�� ���۵Ȱų� �ٸ����Ե�
        canMove = true;
        hookedFishes.Clear(); // ���ð� ���۵Ǹ� List Ŭ����
    }

    private void StopFishing()
    {
        Debug.Log("StopFishing");
        canMove = false; // ���ô� ������ �� ����
        cameraTween.Kill(false); // �� �ڵ� ������ StopFishing() �޼��� ���ȣ���, true�ָ� -25 �Ѿ�� �������� �ٷ� ��������
        cameraTween = mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            coll.enabled = true;
            
            int num = 0;
            for (int i = 0; i < hookedFishes.Count; i++)
            {
                hookedFishes[i].transform.SetParent(null); // ���ڸ��� �ǵ�������
                hookedFishes[i].ResetFish(); // ���ڸ��� �ǵ�������
                num += hookedFishes[i].Type.price; // ������ ���
            }

            IdleManager.instance.totalGain = num;
            ScreensManager.instance.ChangeScreen(Screens.END);
        });
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        // strength�� 3�̹Ƿ� �ѹ��� �ִ� 3���������� ���� �� ����
        if (target.CompareTag("Fish") && fishCount != strength)
        {
            fishCount++;
            Fish component = target.GetComponent<Fish>();
            component.Hooked(); // ���� fish�� �ݶ��̴� ��Ȱ��ȭ�ؼ� OnTriggerEnter2D �ٽ� �������ʵ���
            hookedFishes.Add(component);
            target.transform.SetParent(transform); // ���� ������ hook�� ���󰡵���
            target.transform.position = hookedTransform.position; // ���� ������ hook�� ���󰡵���
            target.transform.rotation = hookedTransform.rotation; // hook�� 90�� ȸ���� �����̹Ƿ� ���� fish�� 90�� ȸ���ϰԵ�
            target.transform.localScale = Vector3.one; // ������ x���� -1�϶� 90�� ȸ���ϸ� �ֵ��̰� �Ʒ��� ���ع����Ƿ�

            // 90�� ȸ���� ���¿��� shake 5�ʵ����ϴٰ�
            target.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                target.transform.rotation = Quaternion.identity; // ���� ȸ��������
            });

            if (fishCount == strength) StopFishing(); // 3���� ������ ��������
        }
    }
}
