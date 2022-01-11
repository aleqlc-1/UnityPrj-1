using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Start()
    {
        // ������ ���ο� �پ��ִ� ��ũ��Ʈ�̹Ƿ� ������ ������ŭ coinCount������ ���� +1��
        GameplayManager.instance.SetCoinCount(1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            GameplayManager.instance.SetCoinCount(-1);
            AudioManager.instance.PlayCoinSound();
            gameObject.SetActive(false);
        }
    }
}
