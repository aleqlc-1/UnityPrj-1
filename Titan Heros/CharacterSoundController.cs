using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    public AudioSource playerAttackSound;

    // �÷��̾��� �븻���� �ִϸ��̼��� �̺�Ʈ�� ����Ͽ� �����Ҷ� �Ҹ�������
    private void Play_PlayerAttackSound()
    {
        playerAttackSound.Play();
    }
}
